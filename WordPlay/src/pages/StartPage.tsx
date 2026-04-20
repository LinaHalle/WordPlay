import { useNavigate } from "react-router-dom";
import Card from "../components/Card";
import Button from "../components/Button";
import { useState } from "react";

import "../index.css";


export default function StartPage() {
  const navigate = useNavigate();
  const [showRules, setShowRules] = useState(false);
  const [username, setUsername] = useState<string>('');
  return (
    <div className="startpage">
      <h1 className="title">BRAINFART</h1>

      <Card className="main-card">
        <input
          className="input"
          placeholder="ENTER NAME"
          value={username}
          onChange={(e) => setUsername(e.target.value)}  
        />
        <Button
        className="HostButton"
          disabled={!username}
          onClick={() => {
            const playerId = crypto.randomUUID();

            const player = {
              username,
              playerId,
              isHost: true
            };
            localStorage.setItem("currentPlayer",JSON.stringify(player));
            navigate("/ruleSet");
          }}
          >
          HOST GAME
        </Button>
        <p>Want to join a game? Paste link below</p>
       <input
  className="btn join-input"
  placeholder="PASTE LINK"
  onKeyDown={(e) => {
    if (e.key === "Enter") {
      const value = (e.target as HTMLInputElement).value;

      // om du klistrar full länk → extrahera id
      const gameId = value.split("/").pop();

      navigate(`/lobby/${gameId}`);
    }
  }}
/>
      </Card>

      <p className="help-text">Don't know the rules?</p>

      <Button className="body-btn" onClick={() => setShowRules(true)}>
        RULES
      </Button>
      {showRules && (
  <div className="modal-overlay" onClick={() => setShowRules(false)}>
    <div className="modal" onClick={(e) => e.stopPropagation()}>
   <h2>Brainfart Rules</h2>

<p>
  Brainfart is a word game where players fill categories based on a random letter under time pressure.<br />
  <br />

  Setup: The Host choose at least 3 categories (e.g. Name, Country, City) and number of rounds.<br />
  <br />

  Round: A random letter is generated. All answers must start with that letter.<br />
  <br />

  Task: Each player enters one word per category starting with the letter.<br />
  <br />

  End: The round ends when a player presses Done. A 5 second timer starts and then all answers are locked and compared.<br />
  <br />

  Scoring: Different answers = 10 points each, single answer = 20 points, same answer = 5 points, empty = 0.<br />
  <br />

  Goal: Highest total score after all rounds wins Brainfart.
</p>

      <Button onClick={() => setShowRules(false)}>CLOSE</Button>
    </div>
  </div>
)}
    </div>
    
  );
}



StartPage.route = {
  path: '/',
  menuLabel: 'StartPage',
  index: 1
}