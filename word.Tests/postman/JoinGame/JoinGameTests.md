# API tests WordPlay

## POST /games/{{gameId}}/join

### Join game with valid name
{{base_url}}/games/{{gameId}}/join

Body:
```json
{
    "name": "Player2"
}
```

#### Test 1: Join was successful
pm.test("Status code is 200", function() {
    pm.response.to.have.status(200);
});

#### Test 2: Response contains gameId and playerId
pm.test("Svar inehaller gameId och playerId", function() {
    const data = pm.response.json();
    pm.expect(data).to.have.property("gameId");
    pm.expect(data).to.have.property("playerId");
});

### Save env data
if (pm.response.code === 200) {
    const data = pm.response.json();
    pm.environment.set("playerId2", data.playerId);
}


### Join game without name
{{base_url}}/games/{{gameId}}/join

Body:
```json
{
    "name": ""
}
```

#### Test 1: Empty name gives 400
pm.test("Kan inte joina utan playerName, ska ge 400", function() {
    pm.response.to.have.status(400);
});


### Join non-existent game
{{base_url}}/games/00000000-0000-0000-0000-000000000000/join

Body:
```json
{
    "name": "Player2"
}
```

#### Test 1: Non-existent game gives 404
pm.test("Joina ett game som inte finns ska ge 404", function() {
    pm.response.to.have.status(404);
});


### Join already started game
{{base_url}}/games/{{gameId}}/join

Body:
```json
{
    "name": "LatePlayer"
}
```

#### Test 1: Already started gives 400
pm.test("Join started game ska ge 400", function() {
    pm.response.to.have.status(400);
    const data = pm.response.json();
    pm.expect(data).to.equal("Game already started");
});
