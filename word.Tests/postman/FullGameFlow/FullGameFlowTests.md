# API tests WordPlay

## Full Game Flow — 3 players, 3 rounds

### Step 1: Create lobby
POST {{base_url}}/games?hostName=Host1

#### Test: Lobby created
pm.test("Status code is 201", function () {
    pm.response.to.have.status(201);
});

### Save env data
if (pm.response.code === 201) {
    const data = pm.response.json();
    pm.environment.set("gameId", data.gameId);
    pm.environment.set("playerId", data.playerId);
}


### Step 2: Set settings (3 rounds, 6 categories)
POST {{base_url}}/games/{{gameId}}/settings

Body:
```json
{
    "Categories": ["Namn", "Land", "Stad", "Frukt", "Bilmodell", "Fotbollsspelare"],
    "Rounds": 3
}
```

#### Test: Settings accepted
pm.test("Settings accepted, status 200", function() {
    pm.response.to.have.status(200);
});


### Step 3: Player 2 joins
POST {{base_url}}/games/{{gameId}}/join

Body: `{ "name": "Player2" }`

#### Test: Player 2 joined
pm.test("Player 2 joined, status 200", function() {
    pm.response.to.have.status(200);
});

### Save env data
if (pm.response.code === 200) {
    const data = pm.response.json();
    pm.environment.set("playerId2", data.playerId);
}


### Step 4: Player 3 joins
POST {{base_url}}/games/{{gameId}}/join

Body: `{ "name": "Player3" }`

#### Test: Player 3 joined
pm.test("Player 3 joined, status 200", function() {
    pm.response.to.have.status(200);
});

### Save env data
if (pm.response.code === 200) {
    const data = pm.response.json();
    pm.environment.set("playerId3", data.playerId);
}


### Step 5: Host starts Round 1
POST {{base_url}}/games/{{gameId}}/start?playerId={{playerId}}

#### Test: Game started, letter received
pm.test("Game started, status 200", function() {
    pm.response.to.have.status(200);
});

pm.test("Response contains letter", function() {
    const data = pm.response.json();
    pm.expect(data).to.have.property("letter");
});


### Steps 6-8: All 3 players submit answers (Round 1)
POST {{base_url}}/games/{{gameId}}/answers

Each player sends their playerId and answers in the body.

#### Test: Each player's answers accepted
pm.test("Player X answers accepted", function() {
    pm.response.to.have.status(200);
});

#### Test: Last player triggers round finished
pm.test("Round finished after all players submitted", function() {
    const data = pm.response.json();
    pm.expect(data.roundFinished).to.be.true;
});


### Step 9: Finish Round 1
POST {{base_url}}/games/{{gameId}}/finish-round

#### Test: Round finished
pm.test("Round 1 finished, status 200", function() {
    pm.response.to.have.status(200);
});


### Step 10: Check leaderboard after Round 1
GET {{base_url}}/games/{{gameId}}

#### Test: Status is ShowingLeaderboard, roundsLeft is 2
pm.test("Status is ShowingLeaderboard", function() {
    const data = pm.response.json();
    pm.expect(data.status).to.equal("ShowingLeaderboard");
});

pm.test("RoundsLeft is 2 after round 1", function() {
    const data = pm.response.json();
    pm.expect(data.roundsLeft).to.equal(2);
});


### Step 11: Host starts Round 2
POST {{base_url}}/games/{{gameId}}/start?playerId={{playerId}}

Host calls /start again from ShowingLeaderboard state. New letter generated, answers cleared.


### Steps 12-15: Round 2 (submit x3, finish)
Same flow as Round 1.


### Step 16: Check leaderboard after Round 2
GET {{base_url}}/games/{{gameId}}

#### Test: Status is ShowingLeaderboard, roundsLeft is 1
pm.test("Status is ShowingLeaderboard", function() {
    const data = pm.response.json();
    pm.expect(data.status).to.equal("ShowingLeaderboard");
});


### Step 17: Host starts Round 3
POST {{base_url}}/games/{{gameId}}/start?playerId={{playerId}}


### Steps 18-21: Round 3 (submit x3, finish)
Same flow as Round 1.


### Step 22: Check final game state
GET {{base_url}}/games/{{gameId}}

#### Test: Game has ended, no rounds left
pm.test("Game status is GameEnded", function() {
    const data = pm.response.json();
    pm.expect(data.status).to.equal("GameEnded");
});

pm.test("RoundsLeft is 0", function() {
    const data = pm.response.json();
    pm.expect(data.roundsLeft).to.equal(0);
});
