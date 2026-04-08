import type { Host } from "../InterFace/CreateGame";

export async function hostGame(host: Host): Promise<{ gameId: string; playerId: string }> {

    const response = await fetch("/games", {
        method: "Post",
        headers: { "Content-Type": "application/json"},
        body: JSON.stringify(host)
    });
    if (!(await response).ok){
        throw new Error(`Failed to create game: ${(await response).statusText}`)
    }

    const data: { gameId: string, playerId: string } = await response.json();
    return data;
}