import type { Host } from "../InterFace/CreateGame";


export async function hostGame(data: Host) {
  const response = await fetch(`/games`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

export async function setGameSettings(gameId: string, categories: string[], rounds: number, language: string = "en"): Promise<void> {
    const response = await fetch(`/games/${gameId}/settings`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ categories, rounds, language }),
    });
    if (!response.ok) {
        throw new Error(`Failed to set game settings: ${response.statusText}`);
    }
}
