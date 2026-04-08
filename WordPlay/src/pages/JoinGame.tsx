import type { Guest } from "../InterFace/JoinGame";

function JoinGame() {

    async function joinGame(gameID: string, playerName: string) {
        
        const response = await fetch(`/games/${gameID}/join`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
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

}