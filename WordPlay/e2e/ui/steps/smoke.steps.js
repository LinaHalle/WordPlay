import { createBdd } from 'playwright-bdd';
const { Given, When, Then } = createBdd();

Given('att jag öppnar startsidan', async ({ page }) => {
    await page.goto('/');
});

Then('ska jag se sidans titel innehåller {string}', async ({ page }, expected) => {
    const title = await page.title();
    if (!title.includes(expected)) {
        throw new Error(`Expected title to include "${expected}" but got "${title}"`);
    }
});