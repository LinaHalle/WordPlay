# API tests WordPlay - Edge Cases

## Setup requests

The collection starts with 4 setup requests that create a lobby, join a second player, configure settings (Animals, Cities, Names / 3 rounds), and start the game. These populate `{{gameId}}`, `{{hostPlayerId}}`, `{{player2Id}}`, and `{{currentLetter}}`.

---

## 1. Input Boundary Tests

### Test 01: Very long hostname (1000 chars)
POST {{base_url}}/games?hostName=AAA...AAA (1000 A's)

pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});
pm.test("Error mentions name length", function () {
    const body = pm.response.text();
    pm.expect(body).to.include("20 characters or less");
});

### Test 02: Very long player name (1000 chars)
POST {{base_url}}/games/{{gameId}}/join  body: { "name": "AAA...AAA" }

pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});
pm.test("Error mentions name length", function () {
    const body = pm.response.text();
    pm.expect(body).to.include("20 characters or less");
});

### Test 03: XSS in hostname
POST {{base_url}}/games?hostName=<script>alert(1)</script>

pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});
pm.test("Error mentions invalid characters", function () {
    const body = pm.response.text();
    pm.expect(body).to.include("letters, numbers, and spaces");
});

### Test 04: SQL injection in player name
POST {{base_url}}/games/{{gameId}}/join  body: { "name": "'; DROP TABLE--" }

pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});
pm.test("Error mentions invalid characters", function () {
    const body = pm.response.text();
    pm.expect(body).to.include("letters, numbers, and spaces");
});

### Test 05: Emoji in hostname
POST {{base_url}}/games?hostName=Player🎮

pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});
pm.test("Error mentions invalid characters", function () {
    const body = pm.response.text();
    pm.expect(body).to.include("letters, numbers, and spaces");
});

### Test 06: Unicode in player name
POST {{base_url}}/games/{{gameId}}/join  body: { "name": "Spelere" }

pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});
pm.test("Error mentions invalid characters", function () {
    const body = pm.response.text();
    pm.expect(body).to.include("letters, numbers, and spaces");
});

---

## 2. Partial / Mismatched Data

### Test 07: Partial answers (subset of categories)
POST {{base_url}}/games/{{gameId}}/answers  body: only "Animals" key (missing Cities, Names)

pm.test("Status code is 200", function () {
    pm.response.to.have.status(200);
});
pm.test("Partial answers accepted", function () {
    const data = pm.response.json();
    pm.expect(data.roundFinished).to.be.false;
});

### Test 08: Extra categories in answers
POST {{base_url}}/games/{{gameId}}/answers  body: includes "FakeCategory" key

pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});
pm.test("Error mentions invalid categories", function () {
    const body = pm.response.text();
    pm.expect(body).to.include("invalid categories");
});

### Test 09: Unknown playerId in answers
POST {{base_url}}/games/{{gameId}}/answers  body: random GUID as playerId

pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});
pm.test("Error mentions player not found", function () {
    const body = pm.response.text();
    pm.expect(body).to.include("Player not found");
});

### Test 10: Empty JSON body on settings
POST {{base_url}}/games/{{tempGameId}}/settings  body: {}
Pre-request script creates a fresh lobby and saves {{tempGameId}}.

pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});
pm.test("Error mentions categories required", function () {
    const body = pm.response.text();
    pm.expect(body).to.include("Categories are requiered");
});

### Test 11: Empty JSON body on answers
POST {{base_url}}/games/{{gameId}}/answers  body: {}

pm.test("Status code is 400 or 500", function () {
    pm.expect(pm.response.code).to.be.oneOf([400, 500]);
});

---

## 3. State Transition Edge Cases

### Test 12: Finish round twice (double finish)
Pre-request script calls finish-round once (should succeed).
Main request calls finish-round again.

pm.test("Status code is 400 on second finish", function () {
    pm.response.to.have.status(400);
});
pm.test("Error says round not finished yet", function () {
    const body = pm.response.text();
    pm.expect(body).to.include("Round not finished yet");
});

### Test 13: Start game without settings
Pre-request script creates fresh lobby with 2 players but skips settings.

pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});
pm.test("Error mentions categories must be set", function () {
    const body = pm.response.text();
    pm.expect(body).to.include("Categories must be set before starting");
});

### Test 14: Settings after game started
POST {{base_url}}/games/{{gameId}}/settings (game is already InRound/ShowingLeaderboard)

pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});
pm.test("Error says game already started", function () {
    const body = pm.response.text();
    pm.expect(body).to.include("Game already started");
});

---

## 4. Invalid Data Types

### Test 15: Rounds as string instead of number
POST {{base_url}}/games/{{tempGameId}}/settings  body: { "Rounds": "three" }

pm.test("Status code is 400", function () {
    pm.response.to.have.status(400);
});

### Test 16: Non-GUID gameId
POST {{base_url}}/games/not-a-guid/settings

pm.test("Status code is 404 (route constraint rejects non-GUID)", function () {
    pm.response.to.have.status(404);
});

### Test 17: Missing body on answers
POST {{base_url}}/games/{{gameId}}/answers (no body at all)

pm.test("Status code is 400 or 415", function () {
    pm.expect(pm.response.code).to.be.oneOf([400, 415]);
});
