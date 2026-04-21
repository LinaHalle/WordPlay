import { test, expect } from "@playwright/test";


test('Lobby page pressing start no players.', async ({ page }) => {
    //setup from startpage
    await page.goto('localhost:5173/');
    await page.locator(".input").fill("Suicidal Squirl");
    await page.locator(".HostButton").click();

    //setup from ruleset
    await expect(page).toHaveURL('http://localhost:5173/ruleSet');
    await page.locator(".option", { hasText: "4" }).click();
    const categories = ['Animals', 'Fruit', 'Sports'];
    for (const cat of categories) {
        await page.locator('.option', { hasText: cat }).click();
    }
    await page.locator(".body-btn").click();


    //btn test    
    await page.getByRole('button', { name: 'start'}).click();
    expect(page.getByText( 'Need at least 2 players')).toBeVisible();
});