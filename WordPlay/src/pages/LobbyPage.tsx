import { useEffect, useState } from "react";
import "../index.css";
import Card from "../components/Card";
import Button from "../components/Button";
import { useParams } from "react-router-dom";

export default function LobbyPage() {
  const { gameId } = useParams();
  const [username, setUsername] = useState("");

  const [game, setGame] = useState<null | {
    gameId: string;
    hostId: string;
    rounds: number;
    categories: string[];
    players: { playerId: string; userName: string }[];
  }>(null);

  
  const playerId = localStorage.getItem("playerId");

  const isJoined =
    !!game &&
    !!playerId &&
    game?.players?.some(p => p.playerId === playerId);

  const isHost = playerId === game?.hostId;

  // FETCH GAME
useEffect(() => {
  if (!gameId) return;

  let isActive = true;

  const fetchGame = async () => {
    try {
      console.log("FETCHING GAME:", gameId);

      const res = await fetch(`http://localhost:5095/games/${gameId}`);
      const data = await res.json();

      console.log("BACKEND DATA:", {
        gameId: data.gameId,
        players: data.players.length,
        hostId: data.hostId,
        status: data.status
      });

      if (!isActive) return;

      setGame(data);
    } catch (err) {
      console.error("FETCH ERROR:", err);
    }
  };

  // direkt första fetch
  fetchGame();

  // polling
  const interval = setInterval(fetchGame, 2000);

  return () => {
    isActive = false;
    clearInterval(interval);
  };
}, [gameId]);

  // LOADING STATE
  if (!game) {
    return (
      <Card className="lobby-card">
        <p>Loading game...</p>
      </Card>
    );
  }

  // JOIN STATE (om spelaren inte är med)
  if (!isJoined) {
    return (
      <Card className="lobby-card">
        <h2>Join Game</h2>

        <input
          value={username}
          className="input"
          placeholder="Enter name"
          onChange={(e) => setUsername(e.target.value)}
        />

        <Button
          disabled={!username}
          onClick={async () => {
            const res = await fetch(
              `http://localhost:5095/games/${game.gameId}/join`,
              {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ name: username })
              }
            );


            
            const data = await res.json();

          localStorage.setItem("playerId", data.playerId);
          

          // hämta uppdaterad game state direkt
          const refreshed = await fetch(`http://localhost:5095/games/${game.gameId}`);
          const updated = await refreshed.json();

          setGame(updated);
          setUsername("");
          }}
        >
          Join Game
        </Button>
      </Card>
    );
  }

  // MAIN LOBBY
  return (
    <div className="startpage">
      <h1 className="title">LOBBY</h1>

      <div className="lobby-wrapper">
        <Card className="lobby-card">
          <h2>Game Info</h2>

          <div className="lobby-grid">
            <div>
              <p>
                <strong>Game ID:</strong> {game.gameId}
              </p>

              <p>
                <strong>Host:</strong>{" "}
                {game.players.find(p => p.playerId === game.hostId)?.userName}
              </p>
            </div>

            <div>
              <p>
                <strong>Rounds:</strong> {game.rounds}
              </p>

              <h3>Categories:</h3>
              <ul className="categories-list">
                {game.categories.map(cat => (
                  <li key={cat}>{cat}</li>
                ))}
              </ul>
            </div>
          </div>
        </Card>

        <Card className="lobby-card">
          <p>Invite link:</p>
          <Button
  onClick={() => {
    const link = `${window.location.origin}/lobby/${game.gameId}`;
    navigator.clipboard.writeText(link);
  }}
>
  Copy invite link
</Button>

          <h2>Players</h2>

          <ul>
            {game.players.map(p => (
              <li key={p.playerId}>
                {p.playerId === game.hostId ? "👑 " : ""}
                {p.userName}
              </li>
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
      </div>
    </div>
  );
}

LobbyPage.route = {
  path: "/lobby",
  menuLabel: "LobbyPage",
  index: 3
};