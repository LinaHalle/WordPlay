import { createBdd } from 'playwright-bdd';

const { Given, When, Then } = createBdd();

Given('I am on the start page', async ({ page }) => {
  await page.goto('http://localhost:5173/');
});

When('I enter a username {string}', async ({ page }, username: string) => {
  await page.fill('input[placeholder="ENTER NAME"]', username);
});

When('I click host game', async ({ page }) => {
  await page.click('button:has-text("HOST GAME")');
});

Then('I should be redirected to rules page', async ({ page }) => {
  await page.waitForURL('**/ruleSet');
});