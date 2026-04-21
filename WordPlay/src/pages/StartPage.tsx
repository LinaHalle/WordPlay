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
            localStorage.setItem("currentPlayer", JSON.stringify(player));
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
              const val = (e.target as HTMLInputElement).value;
              // Extracts the ID from a link like ".../lobby/abc-123" or just "abc-123"
              const gameId = val.split("/").pop();
              if (gameId) navigate(`/lobby/${gameId}`);
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
            <h2>Rules</h2>
            <p>
              - Regel 1 <br />
              - Regel 2 <br />
              - Regel 3
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
};