# API tests WordPlay

## POST /games

### Create lobby with username
base_url = http://localhost:5095;

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


### Create lobby without username
{{base_url}}/games?hostName= 

#### Test 1: Game was NOT created
pm.test("Tom hostname ska ge 400", function () {
    pm.response.to.have.status(400);
});


### Create env data
if (pm.response.code === 201) {
    const data = pm.response.json();
    pm.environment.set("gameId", data.gameId);
    pm.environment.set("playerId", data.playerId);
}

const data = pm.response.json();
