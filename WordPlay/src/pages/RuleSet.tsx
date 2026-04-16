import Card from "../components/Card";
import Button from "../components/Button";

import { hostGame } from "../services/CreateGame";

import "../index.css";
import { useNavigate } from "react-router-dom";
import { useState } from "react";

export default function RuleSet() {
  const navigate = useNavigate();
  const [rounds, setRounds] = useState<number | null>(null);
  const [categories, setCategories] = useState<string[]>([]);

  return (
    <div className="page-center">
  <h1 className="title" >Ruleset</h1>

  <div className="cards-container">
    <div className="card-section">
      <p>Number of rounds:</p>

      <Card className="long-card">
  {[3,4,5,6,7,8,9,10].map(num => (
    <label className="option" key={num}>
      <span>{num}</span>
      <input
        type="radio"
        name="rounds"
        checked={rounds===num}
        onChange={() => setRounds(num)}  
      />
    </label>
  ))}
</Card>
    </div>

    <div className="card-section">
      <p>Choose categories:</p>

      <Card className="long-card">
  {["Animals", "Fruit", "Sports", "Name", "Country", "City", "CarBrand", "Movies"].map(cat => (
    <label className="option" key={cat}>
      <span>{cat}</span>
      <input
        type="checkbox"
        onChange={(e) => {
          if (e.target.checked) {
            setCategories([...categories, cat]);
          } else {
            setCategories(categories.filter(c => c !== cat));
          }
        }}
      />
    </label>
  ))}
</Card>
    </div>
  </div>

      <Button
        className="body-btn"
        disabled={!rounds || categories.length < 3}
        onClick={async () => {
          console.log("clicked create game");
          const stored = localStorage.getItem("currentPlayer");

          if (!stored) {
            navigate("/");
            return;
          }

          const host = JSON.parse(stored);

          if (!host) {
            navigate("/");
            return;
          }

          try {
            const result = await hostGame({
              hostName: host.username,
              categories,
              rounds: rounds!
             
            });

          localStorage.setItem("gameId", result.gameId);
          localStorage.setItem("playerId", result.playerId);

          navigate(`/lobby/${result.gameId}`);
        } catch (err) {
          console.error(err);
        }
      }}
      >
        Create Game
      </Button>
</div>

  );
}



RuleSet.route = {
  path: '/ruleSet',
  menuLabel: 'RuleSet',
  index: 2
}