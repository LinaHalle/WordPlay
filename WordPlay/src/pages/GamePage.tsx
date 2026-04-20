import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import Card from "../components/Card";
import Button from "../components/Button";
import "../index.css";

type Player = {
  playerId: string;
  userName: string;
};

type Game = {
  gameId: string;
  hostId: string;
  rounds: number;
  categories: string[];
  players: Player[];
  currentLetter: string;
  scoreboard: Record<string, number>;
  status: string;
};



export default function GamePage() {
  const { gameId } = useParams();
  const playerId = localStorage.getItem("playerId");

  const [game, setGame] = useState<Game | null>(null);
  const [countdown, setCountdown] = useState(3);
  const [showLetter, setShowLetter] = useState(false);
  const navigate = useNavigate();

  const [answers, setAnswers] = useState<{ [key: string]: string; }>({});
  const [hasSubmitted, setHasSubmitted] = useState(false);
  

  // FETCH GAME (polling)
  useEffect(() => {
    if (!gameId) return;

    const fetchGame = async () => {
      const res = await fetch(`/games/${gameId}`);
      const data = await res.json();
      setGame(data);
    };

    fetchGame();
    const interval = setInterval(fetchGame, 2000);

    return () => clearInterval(interval);
  }, [gameId]);

  useEffect(() => {
  if (game?.status !== "InRound") return;

  setCountdown(3);
  setShowLetter(false);
  setHasSubmitted(false);

  const timer = setInterval(() => {
    setCountdown(prev => {
      if (prev <= 1) {
        clearInterval(timer);
        setShowLetter(true);
        return 0;
      }
      return prev - 1;
    });
  }, 1000);

  return () => clearInterval(timer);
}, [game?.status]);

  // current letter
  const letter = game?.currentLetter;

  // handle input
  const handleChange = (category: string, value: string) => {
    setAnswers(prev => ({
      ...prev,
      [category]: value
    }));
  };

  // auto-submit when another player submits first
  useEffect(() => {
    if (game?.status === "WaitingForAnswers" && !hasSubmitted) {
      submitAnswers();
    }
  }, [game?.status]);

  //submit answers
  const submitAnswers = async () => {
    if (hasSubmitted) return;
    setHasSubmitted(true);
    await fetch(`/games/${gameId}/answers`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        playerId,
        answers
      })
    });

  };
  
  const nextRound = async () => {
    await fetch(`/games/${gameId}/start?playerId=${playerId}`, {
    method: "POST"
    });

  // reset UI state för ny runda
  setAnswers({});
  setHasSubmitted(false);
  setShowLetter(false);
  setCountdown(3);
  };

  const restartGame = async () => {
    await fetch(`/games/${gameId}/restart`, {
      method: "POST"
    });
    setAnswers({});
    setCountdown(3);
    setShowLetter(false);
  };

  const goToStart = () => {
    navigate("/");
  };

  if (!game) {
    return <p>Loading...</p>;
  }

  if (game.status === "InRound" || game.status === "WaitingForAnswers") {
    return (
      <div className="startpage">
        <h1 className="title">
          {!showLetter ? (
            countdown === 3 ? "READY" :
            countdown === 2 ? "SET" :
            "GO!" 
          ) : (
              `LETTER: ${letter}`
          )}
        </h1>

        <div className="lobby-wrapper">
          <Card className="lobby-card">
            <h2>Fill in answers</h2>

            {game.categories.map((cat: string) => (
              <div key={cat} className="answer-row">
                <label>{cat}</label>
                <input
                  className="game-input"
                  disabled={hasSubmitted}
                  value={answers[cat] || ""}
                  onChange={(e) => handleChange(cat, e.target.value)}
                />
              </div>
            ))}

            <Button
              onClick={submitAnswers}
              disabled={hasSubmitted || !game.categories.every(cat =>
                answers[cat]?.trim() && answers[cat].trim().toUpperCase().startsWith(letter?.toUpperCase() ?? "")
              )}
            >
              DONE
            </Button>
          </Card>
        </div>
      </div>
    );
  }

  if (game.status === "ShowingLeaderboard") {
    console.log(game.scoreboard);
     const isHost = game.hostId === playerId;

  return (
    <div className="startpage">
      <h1 className="title">SCOREBOARD</h1>

      <div className="lobby-wrapper">
        <Card className="lobby-card">
          <h2>🏆 Results</h2>

          <ul className="score-list">
            {Object.entries(game.scoreboard || {})
              .sort((a, b) => b[1] - a[1])
              .map(([id, score]) => {
                const player = game.players.find(p => p.playerId === id);

                return (
                  <li key={id} className="score-row">
                    <span className="player-name">
                      <strong>{player?.userName ?? "Unknown"}</strong>{" "}
                    </span>

                    <span className="player-score">
                      {score} pts
                    </span>
                  </li>
                );
              })}
          </ul>

          {isHost && (
            <Button className="next-round-btn"
              onClick={nextRound}>
              Next Round
            </Button>
          )}
        </Card>
      </div>
    </div>
  );
  }
  if (game.status === "GameEnded") {
  const sorted = Object.entries(game.scoreboard || {})
    .sort((a, b) => b[1] - a[1]);

  const winnerId = sorted[0]?.[0];
  const winner = game.players.find(p => p.playerId === winnerId);

  return (
    <div className="startpage">
      <h1 className="title">🎉 GAME FINISHED</h1>

      <div className="lobby-wrapper">
        <Card className="lobby-card">
          <h2>🏆 Final Scores</h2>

          <ul className="score-list">
            {sorted.map(([id, score]) => {
              const player = game.players.find(p => p.playerId === id);

              return (
                <li key={id} className="score-row">
                  <span className="player-name">
                    <strong>{player?.userName ?? "Unknown"}</strong>
                  </span>

                  <span className="player-score">
                    {score} pts
                  </span>
                </li>
              );
            })}
          </ul>

          <h2 className="winner-text">
            🥇 Winner: <strong>{winner?.userName}</strong>
          </h2>
          {game.hostId === playerId && (
            <Button onClick={restartGame}>
              New Game
            </Button>
          )}

          <Button onClick={goToStart}>
            Start Menu
          </Button>
        </Card>
      </div>
    </div>
  );
}
}

GamePage.route = {
  path: "/game/:gameId",
  menuLabel: "GamePage",
  index: 4
};