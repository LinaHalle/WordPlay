import { useState } from "react";

type Category = {
  name: string;
  difficulty: "lätt" | "medel" | "svår";
};

const ALL_CATEGORIES: Category[] = [
  { name: "Namn", difficulty: "lätt" },
  { name: "Land", difficulty: "lätt" },
  { name: "Stad", difficulty: "lätt" },
  { name: "Frukt", difficulty: "lätt" },
  { name: "Bilmodell", difficulty: "medel" },
  { name: "Fotbollsspelare", difficulty: "medel" },
  { name: "Fågel", difficulty: "medel" },
  { name: "Djur", difficulty: "lätt" },
  { name: "Kändis", difficulty: "medel" },
  { name: "Film", difficulty: "medel" },
  { name: "Möbel", difficulty: "svår" },
  { name: "Klädesplagg", difficulty: "medel" },
  { name: "Färg", difficulty: "lätt" },
  { name: "Yrke", difficulty: "medel" },
  { name: "Mat", difficulty: "lätt" },
  { name: "Dryck", difficulty: "lätt" },
  { name: "Blomma", difficulty: "medel" },
  { name: "Verktyg", difficulty: "svår" },
  { name: "Apparat", difficulty: "svår" },
  { name: "Sport", difficulty: "medel" },
  { name: "Låt", difficulty: "svår" },
  { name: "Artist", difficulty: "medel" },
  { name: "Företag", difficulty: "svår" },
  { name: "Spel", difficulty: "medel" },
];

export default function JoinOrCreateGame() {
  const [name, setName] = useState("");
  const [gameId, setGameId] = useState("");
  const [mode, setMode] = useState<"join" | "create" | null>(null);
  const [message, setMessage] = useState("");
  const [selectedCategories, setSelectedCategories] = useState<string[]>([]);

  async function handleCreate() {
    if (!name) return setMessage("Ange användarnamn!");
    if (selectedCategories.length === 0) return setMessage("Välj minst en kategori!");
    // Skicka hostName som query, kategorier som body (array av strängar)
    const res = await fetch(`/games?hostName=${encodeURIComponent(name)}`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(selectedCategories),
    });
    if (res.ok) {
      const data = await res.json();
      setMessage(`Spel skapat! Spel-ID: ${data.gameId}`);
    } else {
      setMessage("Kunde inte skapa spel.");
    }
  }

  async function handleJoin() {
    if (!name || !gameId) return setMessage("Ange både namn och spelkod!");
    const res = await fetch(`/games/${gameId}/join`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ playerName: name }),
    });
    if (res.ok) {
      setMessage("Du har gått med i spelet!");
    } else {
      setMessage("Kunde inte gå med i spelet.");
    }
  }

  return (
    <div style={{ maxWidth: 400, margin: "2rem auto", padding: 20, border: "1px solid #ccc", borderRadius: 8 }}>
      <h2>WordPlay</h2>
      <input
        value={name}
        onChange={e => setName(e.target.value)}
        placeholder="Användarnamn"
        style={{ width: "100%", marginBottom: 8 }}
      />
      <div style={{ display: "flex", gap: 8, marginBottom: 8 }}>
        <button onClick={() => setMode("create")}>Skapa spel</button>
        <button onClick={() => setMode("join")}>Gå med i spel</button>
      </div>
      {mode === "join" && (
        <div style={{ marginBottom: 8 }}>
          <input
            value={gameId}
            onChange={e => setGameId(e.target.value)}
            placeholder="Spelkod"
            style={{ width: "100%", marginBottom: 8 }}
          />
          <button onClick={handleJoin} style={{ width: "100%" }}>Gå med</button>
        </div>
      )}
      {mode === "create" && (
        <>
          <div style={{ maxHeight: 200, overflowY: "auto", marginBottom: 8, border: "1px solid #eee", borderRadius: 4, padding: 8 }}>
            <strong>Välj kategorier:</strong>
            <ul style={{ listStyle: "none", padding: 0, margin: 0 }}>
              {ALL_CATEGORIES.map(cat => (
                <li key={cat.name} style={{ marginBottom: 4 }}>
                  <label>
                    <input
                      type="checkbox"
                      checked={selectedCategories.includes(cat.name)}
                      onChange={e => {
                        setSelectedCategories(sel =>
                          e.target.checked
                            ? [...sel, cat.name]
                            : sel.filter(n => n !== cat.name)
                        );
                      }}
                    />
                    {cat.name} <span style={{ fontSize: 12, color: '#888' }}>({cat.difficulty})</span>
                  </label>
                </li>
              ))}
            </ul>
          </div>
          <button onClick={handleCreate} style={{ width: "100%", marginBottom: 8 }}>Skapa</button>
        </>
      )}
      {message && <div style={{ marginTop: 12, color: "#333" }}>{message}</div>}
    </div>
  );
}
