import type { Host } from "../InterFace/CreateGame";

const API_URL = "https://brainfart.onrender.com";

export async function hostGame(data: CreateGameRequest) {
  const response = await fetch(`${API_URL}/games`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (!response.ok) {
    throw new Error(`Failed to create game: ${response.statusText}`);
  }

    const data: { gameId: string, playerId: string } = await response.json();
    return data;
}

export async function setGameSettings(gameId: string, categories: string[], rounds: number): Promise<void> {
    const response = await fetch(`/games/${gameId}/settings`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ categories, rounds }),
    });
    if (!response.ok) {
        throw new Error(`Failed to set game settings: ${response.statusText}`);
    }
}
