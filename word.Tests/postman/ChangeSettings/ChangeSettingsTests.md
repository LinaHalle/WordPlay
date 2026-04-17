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

#### Test 2: Rounds were accepted
pm.test("Rounds valda ger 200", function() {
    pm.response.to.have.status(200);
});


### Change settings with empty categories
{{base_url}}/games/{{gameId}}/settings

Body:
```json
{
    "Categories": [],
    "Rounds": 4
}
```

#### Test 1: Empty categories gives 400
pm.test("Inga categories valda ska ge 400", function() {
    pm.response.to.have.status(400);
});


### Change settings with zero rounds
{{base_url}}/games/{{gameId}}/settings

Body:
```json
{
    "Categories": ["Namn", "Land", "Stad", "Frukt", "Bilmodell", "Fotbollsspelare"],
    "Rounds": 0
}
```

#### Test 1: Zero rounds gives 400
pm.test("Inga rounds valda ska ge 400", function() {
    pm.response.to.have.status(400);
});
