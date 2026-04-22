# API tests WordPlay

## POST /games

### Create lobby with username
{{base_url}}/games?hostName=Username

#### Test 1: Lobby was created
pm.test("Status code is 201", function () {
    pm.response.to.have.status(201);
});

#### Test 2: Lobby returns gameId + playerId
pm.test("Svar inehåller gameId och playerId", function() {
    const data = pm.response.json();
    pm.expect(data).to.have.property("gameId");
    pm.expect(data).to.have.property("playerId");
});

#### Test 3: Status is WaitingForPlayers
pm.test("Status is WaitingForPlayers", function() {
    const data = pm.response.json();
    pm.expect(data.status).to.equal("WaitingForPlayers");
});

### Save env data
if (pm.response.code === 201) {
    const data = pm.response.json();
    pm.environment.set("gameId", data.gameId);
    pm.environment.set("playerId", data.playerId);
}


### Create lobby without username
{{base_url}}/games?hostName=

#### Test 1: Game was NOT created
pm.test("Tom hostname ska ge 400", function () {
    pm.response.to.have.status(400);
});


### Create lobby with whitespace-only hostname
{{base_url}}/games?hostName=%20%20%20

#### Test 1: Whitespace hostname gives 400
pm.test("Whitespace hostname ska ge 400", function () {
    pm.response.to.have.status(400);
});
