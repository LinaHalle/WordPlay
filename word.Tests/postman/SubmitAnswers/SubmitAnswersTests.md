# API tests WordPlay

## POST /games/{{gameId}}/answers

### Submit valid answers (non-last player)
{{base_url}}/games/{{gameId}}/answers

Body:
```json
{
    "playerId": "{{playerId}}",
    "answers": {
        "Namn": "Vera", "Land": "Vatikanstaten", "Stad": "Venice",
        "Frukt": "Vindruva", "Bilmodell": "Volvo", "Fotbollsspelare": "Vialli"
    }
}
```

#### Test 1: Answers accepted
pm.test("Status code is 200", function() {
    pm.response.to.have.status(200);
});

#### Test 2: Round not finished yet (more players need to submit)
pm.test("roundFinished is false", function() {
    const data = pm.response.json();
    pm.expect(data.roundFinished).to.be.false;
});

#### Test 3: Status is WaitingForAnswers
pm.test("Status is WaitingForAnswers", function() {
    const data = pm.response.json();
    pm.expect(data.status).to.equal("WaitingForAnswers");
});


### Submit valid answers (last player)
Body uses playerId2

#### Test 1: Round finished
pm.test("roundFinished is true", function() {
    const data = pm.response.json();
    pm.expect(data.roundFinished).to.be.true;
});

#### Test 2: Status is RoundFinished
pm.test("Status is RoundFinished", function() {
    const data = pm.response.json();
    pm.expect(data.status).to.equal("RoundFinished");
});


### Submit to non-existent game
{{base_url}}/games/00000000-0000-0000-0000-000000000000/answers

#### Test 1: Non-existent game gives 404
pm.test("Should return 404 for non-existent game", function() {
    pm.response.to.have.status(404);
});


### Submit duplicate answers (same player twice)
#### Test 1: Duplicate answers gives 400
pm.test("Should not allow overwriting existing answers", function() {
    pm.response.to.have.status(400);
});


### Submit when game not started (WaitingForPlayers)
#### Test 1: Not started gives 400
pm.test("Round not active when game not started", function() {
    pm.response.to.have.status(400);
});


### Submit when ShowingLeaderboard
#### Test 1: ShowingLeaderboard gives 400
pm.test("Round not active during leaderboard", function() {
    pm.response.to.have.status(400);
});


### Submit when GameEnded
#### Test 1: GameEnded gives 400
pm.test("Round not active when game ended", function() {
    pm.response.to.have.status(400);
});


### Submit with empty answers dict
Body: `{ "playerId": "{{playerId}}", "answers": {} }`

#### Test 1: Empty answers still accepted (200)
pm.test("Empty answers accepted", function() {
    pm.response.to.have.status(200);
});
