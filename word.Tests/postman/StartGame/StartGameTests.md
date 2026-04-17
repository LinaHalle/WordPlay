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


### Start game with only 1 player
{{base_url}}/games/{{gameId}}/start?playerId={{playerId}}

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
