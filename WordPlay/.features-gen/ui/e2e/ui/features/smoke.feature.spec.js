// Generated from: e2e/ui/features/smoke.feature
import { test } from "playwright-bdd";

test.describe('Smoke', () => {

  test('Startsidan går att öppna', async ({ Given, Then, page }) => { 
    await Given('att jag öppnar startsidan', null, { page }); 
    await Then('ska jag se sidans titel innehåller "MinApplikation"', null, { page }); 
  });

});

// == technical section ==

test.use({
  $test: [({}, use) => use(test), { scope: 'test', box: true }],
  $uri: [({}, use) => use('e2e/ui/features/smoke.feature'), { scope: 'test', box: true }],
  $bddFileData: [({}, use) => use(bddFileData), { scope: "test", box: true }],
});

const bddFileData = [ // bdd-data-start
  {"pwTestLine":6,"pickleLine":3,"tags":[],"steps":[{"pwStepLine":7,"gherkinStepLine":4,"keywordType":"Context","textWithKeyword":"Given att jag öppnar startsidan","stepMatchArguments":[]},{"pwStepLine":8,"gherkinStepLine":5,"keywordType":"Outcome","textWithKeyword":"Then ska jag se sidans titel innehåller \"MinApplikation\"","stepMatchArguments":[{"group":{"start":35,"value":"\"MinApplikation\"","children":[{"start":36,"value":"MinApplikation","children":[{"children":[]}]},{"children":[{"children":[]}]}]},"parameterTypeName":"string"}]}]},
]; // bdd-data-end