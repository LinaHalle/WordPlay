# API tests WordPlay

## POST /games/{{gameId}}/settings

### Change settings with valid categories and rounds
{{base_url}}/games/{{gameId}}/settings

Body:
```json
{
    "Categories": ["Namn", "Land", "Stad", "Frukt", "Bilmodell", "Fotbollsspelare"],
    "Rounds": 3
}
```

#### Test 1: Settings were accepted
pm.test("categories valda ska ge 200", function() {
    pm.response.to.have.status(200);
});

#### Test 2: Response contains gameId and status
pm.test("Response contains gameId and status", function() {
    const data = pm.response.json();
    pm.expect(data).to.have.property("gameId");
    pm.expect(data).to.have.property("status");
});


### Change settings with empty categories
Body: `{ "Categories": [], "Rounds": 4 }`

#### Test 1: Empty categories gives 400
pm.test("Inga categories valda ska ge 400", function() {
    pm.response.to.have.status(400);
});


### Change settings with zero rounds
Body: `{ "Categories": ["Namn", ...], "Rounds": 0 }`

#### Test 1: Zero rounds gives 400
pm.test("Inga rounds valda ska ge 400", function() {
    pm.response.to.have.status(400);
});


### Change settings with negative rounds
Body: `{ "Categories": ["Namn", ...], "Rounds": -1 }`

#### Test 1: Negative rounds gives 400
pm.test("Negativa rounds ska ge 400", function() {
    pm.response.to.have.status(400);
});


### Change settings on non-existent game
{{base_url}}/games/00000000-0000-0000-0000-000000000000/settings

#### Test 1: Non-existent game gives 404
pm.test("Non-existent game ska ge 404", function() {
    pm.response.to.have.status(404);
});


### Change settings after game started
Requires setup: create lobby, join player, start game, then try settings.

#### Test 1: Settings after game started gives 400
pm.test("Settings after game started ska ge 400", function() {
    pm.response.to.have.status(400);
});


### Overwrite settings (call settings twice)
Call settings twice on same game in WaitingForPlayers.

#### Test 1: Second settings call also gives 200
pm.test("Overwrite settings ska ge 200", function() {
    pm.response.to.have.status(200);
});
