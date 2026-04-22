import type { Guest } from "../InterFace/JoinGame";

const API_URL = "https://brainfart.onrender.com";

export async function joinGame(gameID: string, playerName: string) {
        
    const response = await fetch(`${API_URL}/games/${gameID}/join`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json'},
        body: JSON.stringify({ playerName }),
    });
    if(response.status === 404){
        throw new Error('Game not found');
    }
    if (response.status === 400) {
        const errorText = await response.text();
        throw new Error(errorText);
    }
    const data: Guest = await response.json();
    return data;
}
