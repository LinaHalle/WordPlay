import type { CreateGameRequest } from "../InterFace/CreateGame";

export async function hostGame(data: CreateGameRequest): Promise<{ gameId: string; playerId: string }> {

    const response = await fetch("/games", {
        method: "POST",
        headers: { "Content-Type": "application/json"},
        body: JSON.stringify(data)
    });
  
    if (!response.ok){
      throw new Error(`Failed to create game: ${response.statusText}`);
    }

  return response.json();
}