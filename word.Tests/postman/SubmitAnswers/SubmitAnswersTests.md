# API tests WordPlay

## POST /games/{{gameId}}/answers

### Submit valid answers
{{base_url}}/games/{{gameId}}/answers

Body:
```json
{
    "playerId": "{{playerId}}",
    "answers": {
        "Namn": "Vera",
        "Land": "Vatikanstaten",
        "Stad": "Venice",
        "Frukt": "Vindruva",
        "Bilmodell": "Volvo",
        "Fotbollsspelare": "Vialli"
    }
}
```

#### Test 1: Answers accepted
pm.test("Status code is 200", function() {
    pm.response.to.have.status(200);
});


### Submit answers to non-existent game
{{base_url}}/games/00000000-0000-0000-0000-000000000000/answers

#### Test 1: Non-existent game gives 404
pm.test("Should return 404 for non-existent game", function() {
    pm.response.to.have.status(404);
});


### Submit duplicate answers
{{base_url}}/games/{{gameId}}/answers

Body:
```json
{
    "playerId": "{{playerId}}",
    "answers": {
        "Namn": "Vera",
        "Land": "Vatikanstaten",
        "Stad": "Venice",
        "Frukt": "Vindruva",
        "Bilmodell": "Volvo",
        "Fotbollsspelare": "Vialli"
    }
}
```

#### Test 1: Duplicate answers gives 400
pm.test("Should not allow overwriting existing answers", function() {
    pm.response.to.have.status(400);
});
