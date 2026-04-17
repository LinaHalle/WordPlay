# API tests WordPlay

## POST /games/{{gameId}}/finish-round

### Finish round when all players submitted
{{base_url}}/games/{{gameId}}/finish-round

#### Test 1: Round finished successfully
pm.test("Should give 200 when round is finished", function() {
    pm.response.to.have.status(200);
});


### Finish round on non-existent game
{{base_url}}/games/00000000-0000-0000-0000-000000000000/finish-round

#### Test 1: Non-existent game gives 404
pm.test("Should give 404 if game doesnt exist", function() {
    pm.response.to.have.status(404);
});


### Finish round when not all players submitted
{{base_url}}/games/{{gameId}}/finish-round

#### Test 1: Not finished yet gives 400
pm.test("Should give 400 trying to finish a round that is not finished", function() {
    pm.response.to.have.status(400);
});
