import Card from "../components/Card";
import Button from "../components/Button";

import { hostGame, setGameSettings } from "../services/CreateGame";

import "../index.css";
import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";

export default function RuleSet() {
  const navigate = useNavigate();
  const [rounds, setRounds] = useState<number | null>(null);
  const [categories, setCategories] = useState<string[]>([]);
  const [language, setLanguage] = useState<string>("en");
  const [availableCategories, setAvailableCategories] = useState<string[]>([]);

  const fallbackCategories: Record<string, string[]> = {
    en: ["Animals", "Fruit", "Sports", "Name", "Country", "City", "CarBrand", "Movies"],
    se: ["Djur", "Frukt", "Sport", "Namn", "Land", "Stad", "Bilmärke", "Filmer"],
  };

  useEffect(() => {
    fetch(`/categories/${language}`)
      .then(res => {
        if (!res.ok) throw new Error("Failed to fetch");
        return res.json();
      })
      .then(data => {
        setAvailableCategories(data);
        setCategories([]);
      })
      .catch(() => {
        setAvailableCategories(fallbackCategories[language] || fallbackCategories.en);
        setCategories([]);
      });
  }, [language]);

  return (
    <div className="page-center">
  <h1 className="title" >Ruleset</h1>

  <div className="cards-container">
    <div className="card-section">
      <p>Language:</p>
      <Card className="long-card">
        {[{ code: "en", label: "English" }, { code: "se", label: "Svenska" }].map(lang => (
          <label className="option" key={lang.code}>
            <span>{lang.label}</span>
            <input
              type="radio"
              name="language"
              checked={language === lang.code}
              onChange={() => setLanguage(lang.code)}
            />
          </label>
        ))}
      </Card>
    </div>

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
  {availableCategories.map(cat => (
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

          if (!rounds) return;

          try {
            const result = await hostGame({
              hostName: host.username,
            });

            await setGameSettings(result.gameId, categories, rounds, language);

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