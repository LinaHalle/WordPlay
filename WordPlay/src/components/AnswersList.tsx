type AnswersListProps = {
  answers: Record<string, Record<string, string>>;
  players: { playerId: string; userName: string }[];
  categories: string[];
};

export default function AnswersList({ answers, players, categories }: AnswersListProps) {
  return (
    <div className="answers-list">
      {categories.map((category) => (
        <div key={category} className="answers-category">
          <h3>{category}</h3>
          <ul>
            {players.map((player) => {
              const answer = answers[player.playerId]?.[category];
              return (
                <li key={player.playerId} className="answer-row">
                  <span className="player-name">{player.userName}:</span>{" "}
                  <span className="player-answer">{answer || "-"}</span>
                </li>
              );
            })}
          </ul>
        </div>
      ))}
    </div>
  );
}
