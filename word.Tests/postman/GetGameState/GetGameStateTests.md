# API tests WordPlay

## GET /games/{{gameId}}

### Get valid game state
{{base_url}}/games/{{gameId}}

#### Test 1: Status code is 200
pm.test("Status code is 200", function() {
    pm.response.to.have.status(200);
});

#### Test 2: Response contains all expected fields
pm.test("Response contains all game state fields", function() {
    const data = pm.response.json();
    pm.expect(data).to.have.property("gameId");
    pm.expect(data).to.have.property("status");
    pm.expect(data).to.have.property("players");
    pm.expect(data).to.have.property("categories");
    pm.expect(data).to.have.property("currentLetter");
    pm.expect(data).to.have.property("answers");
    pm.expect(data).to.have.property("scoreboard");
    pm.expect(data).to.have.property("rounds");
    pm.expect(data).to.have.property("roundsLeft");
});

#### Test 3: Status is a string (not a number)
pm.test("Status is a string", function() {
    const data = pm.response.json();
    pm.expect(data.status).to.be.a("string");
});


### Get non-existent game
{{base_url}}/games/00000000-0000-0000-0000-000000000000

#### Test 1: Non-existent game gives 404
pm.test("Non-existent game ska ge 404", function() {
    pm.response.to.have.status(404);
});
