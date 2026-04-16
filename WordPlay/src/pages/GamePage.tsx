import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Card from "../components/Card";
import Button from "../components/Button";

export default function GamePage() {
  const { gameId } = useParams();
  const playerId = localStorage.getItem("playerId");

  const [game, setGame] = useState<any>(null);
  const [answers, setAnswers] = useState<{ [key: string]: string }>({});
  const [submitted, setSubmitted] = useState(false);

  // FETCH GAME (polling)
  useEffect(() => {
    if (!gameId) return;

    const fetchGame = async () => {
      const res = await fetch(`http://localhost:5095/games/${gameId}`);
      const data = await res.json();
      setGame(data);
    };

    fetchGame();
    const interval = setInterval(fetchGame, 2000);

    return () => clearInterval(interval);
  }, [gameId]);

  if (!game) return <p>Loading...</p>;

  // 🔤 current letter
  const letter = game.currentLetter;

  // ✍️ handle input
  const handleChange = (category: string, value: string) => {
    setAnswers(prev => ({
      ...prev,
      [category]: value
    }));
  };

  // ✅ submit answers
  const submitAnswers = async () => {
    await fetch(`http://localhost:5095/games/${gameId}/answers`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        playerId,
        answers
      })
    });

    setSubmitted(true);
  };

  return (
    <div className="startpage">
      <h1 className="title">LETTER: {letter}</h1>

      <div className="lobby-wrapper">
        <Card className="lobby-card">
          <h2>Fill in answers</h2>

          {game.categories.map((cat: string) => (
            <div key={cat} className="answer-row">
              <label>{cat}</label>
              <input
                className="game-input"
                disabled={submitted}
                value={answers[cat] || ""}
                onChange={(e) => handleChange(cat, e.target.value)}
              />
            </div>
          ))}

          {!submitted ? (
            <Button onClick={submitAnswers}>
              DONE
            </Button>
          ) : (
            <p>Waiting for other players...</p>
          )}
        </Card>
      </div>
    </div>
  );
}

GamePage.route = {
  path: "/game/:gameId",
  menuLabel: "GamePage",
  index: 4
};