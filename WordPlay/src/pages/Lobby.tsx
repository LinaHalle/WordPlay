import React ,{useEffect, useState} from "react";
import type Player from "../InterFace/PlayerInter";




//Lobby showing players in a game
const Lobby : React.FC = () => {
    
    //get saved gameId and playerId from localstorage
    const [gameId, setGameId] = useState<string | null>(localStorage.getItem("gameId"));
    const [playerId, setPlayerId] = useState<string | null>(localStorage.getItem("playerId"));

    //state for all players in lobby
    const [players, setPlayers] = useState<Player[]>([]);

    // runs when gameid or playerid changes
    useEffect(() => {
        //if game or player id missing do nothing
        if(!gameId || !playerId)
            {
                return;
            }
            
            //create curplayer
            const curPlayer: Player = {playerId};
            //current player gets added to state
        setPlayers([curPlayer]);
        
        
    },[gameId, playerId]);

        //return lobby 
    return (
        <div>
            <h1>Lobby</h1>
            <p>Game ID: {gameId}</p>
            <p>Your Player ID: {playerId}</p>
            <h2>Players in lobby:</h2>
            <ul>
                {players.map((player) => (
                    <li key={player.playerId}>{player.name || player.playerId}</li>
                ))} 
            </ul>
            <p>Waiting for players....</p>
        </div>
    );
};

export default Lobby;

