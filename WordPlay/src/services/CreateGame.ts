import type { Host } from "../InterFace/CreateGame";

export async function hostGame(host: Host): Promise<{ gameId: string; playerId: string }> {
    const response = await fetch(`/games`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ hostName: host.hostName }),
    });
    if (!response.ok){
        throw new Error(`Failed to create game: ${response.statusText}`)
    }

    const data: { gameId: string, playerId: string } = await response.json();
    return data;
}

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