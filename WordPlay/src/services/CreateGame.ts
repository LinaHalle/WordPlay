import type { Host } from "../InterFace/CreateGame";

const API_URL = "https://brainfart.onrender.com";

export async function hostGame(data: Host) {
  const response = await fetch(`${API_URL}/games`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (!response.ok) {
    throw new Error(`Failed to create game: ${response.statusText}`);
  }

  return response.json();
}