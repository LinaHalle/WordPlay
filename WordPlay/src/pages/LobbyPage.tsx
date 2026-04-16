import {useEffect, useState} from "react";

import "../index.css";
import Card from "../components/Card";
import Button from "../components/Button";




export default function LobbyPage() {
  //get saved gameId and playerId from localstorage
const [game, setGame] = useState<null | {
  gameId: string;
  hostId: string;
  rounds: number;
  categories: string[];
  players: { playerId: string; name: string }[];
}>(null);
  
  
useEffect(() => {
  const gameId = localStorage.getItem("gameId");

  if (!gameId) return;

  fetch(`${import.meta.env.VITE_API_URL}/games/${gameId}`)
  .then(res => res.json())
  .then(data => {
    console.log("BACKEND DATA:", data);
    setGame(data);
  })
  .catch(err => console.error(err));
}, []);
  
  const playerId = localStorage.getItem("playerId");
  const isHost = playerId === game?.hostId;
  

    //return lobby 
 return (
  <div className="startpage">
    <h1 className="title">LOBBY</h1>
    <div className="lobby-wrapper">
    {!game ? (
      <Card className="lobby-card">
        <p>Loading game...</p>
      </Card>
    ) : (
      <>
        <Card className="lobby-card">
          <h2>Game Info</h2>
          
          <div className="lobby-grid">
            <div>
          <p><strong>Game ID:</strong> {game.gameId}</p>

          <p>
            <strong>Host:</strong>{" "}
            {game.players?.find(p => p.playerId === game.hostId)?.name}
          </p>
          </div>
          <div>
          <p><strong>Rounds:</strong> {game.rounds}</p>

          <h3>Categories:</h3>
          <ul className="categories-list">
            {game.categories.map((cat: string) => (
              <li key={cat}>{cat}</li>
            ))}
          </ul>
          </div>
          </div>
        </Card>

        <Card className="lobby-card">
          <p>Invite link:</p>
          <input
            className="input"
            value={`${window.location.origin}/lobby/${game.gameId}`}
            readOnly
          />
          <h2>Players</h2>

          <ul>
            {game.players?.map((p: any) => (
              <li key={p.playerId}>{p.name}</li>
            ))}
          </ul>

          <p>Waiting for players...</p>

          {isHost && (
            <Button
              onClick={async () => {
                const res = await fetch(
                  `http://localhost:5095/games/${game.gameId}/start`,
                  { method: "POST" }
                );

                const data = await res.json();
                console.log("Game started:", data);
              }}
            >
              START GAME
            </Button>
          )}
        </Card>
      </>
    )}
    </div>
  </div>
);
  };

LobbyPage.route = {
  path: '/lobby',
  menuLabel: 'LobbyPage',
  index: 3
}

