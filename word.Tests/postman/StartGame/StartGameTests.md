# API tests WordPlay

## POST /games/{{gameId}}/start

### Start game with 2 players (host starts)
{{base_url}}/games/{{gameId}}/start?playerId={{playerId}}

#### Test 1: Game started successfully
pm.test("Should give 200 when 2 players and host starts", function() {
    pm.response.to.have.status(200);
});

#### Test 2: Response contains letter
pm.test("Response contains letter", function() {
    const data = pm.response.json();
    pm.expect(data).to.have.property("letter");
});

#### Test 3: Status is InRound
pm.test("Status is InRound", function() {
    const data = pm.response.json();
    pm.expect(data.status).to.equal("InRound");
});

#### Test 4: Letter is a single character
pm.test("Letter is a single character", function() {
    const data = pm.response.json();
    pm.expect(data.letter).to.have.lengthOf(1);
});


### Start game with only 1 player
#### Test 1: Too few players gives 400
pm.test("Start game with only one player ska ge 400", function() {
    pm.response.to.have.status(400);
});


### Non-host tries to start
{{base_url}}/games/{{gameId}}/start?playerId={{playerId2}}

#### Test 1: Non-host start gives 400
pm.test("Only host can start the game, ska ge 400", function() {
    pm.response.to.have.status(400);
});


### Start non-existent game
{{base_url}}/games/00000000-0000-0000-0000-000000000000/start?playerId={{playerId}}

#### Test 1: Non-existent game gives 404
pm.test("Non-existent game ska ge 404", function() {
    pm.response.to.have.status(404);
});


### Start when already InRound
#### Test 1: Already in round gives 400
pm.test("Already InRound ska ge 400", function() {
    pm.response.to.have.status(400);
});

#### Test 2: Error message
pm.test("Error message is correct", function() {
    const data = pm.response.json();
    pm.expect(data).to.equal("Game is not in a startable state");
});


### Start when GameEnded
#### Test 1: GameEnded gives 400
pm.test("GameEnded ska ge 400", function() {
    pm.response.to.have.status(400);
});
