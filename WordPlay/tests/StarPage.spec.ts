import { test, expect } from "@playwright/test";

test('has title', async ({ page }) => {
  await page.goto('localhost:5173');

  // Expect a title "to contain" a substring.
  await expect(page).toHaveTitle(/.+/);
});

test('Host game with username', async ({ page }) => {
  await page.goto('localhost:5173');


  await page.locator(".input").fill("Peter");
  await page.locator(".HostButton").click();
});


test('No username Host game', async ({ page }) => {
    await page.goto('localhost:5173');
  
    await page.locator(".input").fill("");
    await expect(page.locator(".HostButton")).toBeDisabled();
    });