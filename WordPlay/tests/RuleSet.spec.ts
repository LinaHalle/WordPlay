import { test, expect } from "@playwright/test";


test('Click rounds and categories', async ({ page }) => {
  await page.goto('localhost:5173/ruleSet');

  await page.locator(".option", { hasText: "4" }).click();
  await page.locator(".option", { hasText: "Animals" }).click();

  const roundsInput = page.locator('.option', { hasText: "4" }).locator('input');
  await expect(roundsInput).toBeChecked();

  const animalsCheckbox = page.locator('.option', { hasText: 'Animals' }).locator('input');
  await expect(animalsCheckbox).toBeChecked();
  
});


test('select categories no rounds', async ({ page }) => {
  await page.goto('http://localhost:5173/ruleSet');

  const categories = ['Animals', 'Fruit', 'Sports'];

  for (const cat of categories) {
    await page.locator('.option', { hasText: cat }).click();
  }

  for (const cat of categories) {
    await expect(
      page.locator('.option', { hasText: cat }).locator('input')
    ).toBeChecked();
  }
  await expect(page.locator(".body-btn")).toBeDisabled();
});

test('Select rounds no categories', async ({ page }) => {
  await page.goto('localhost:5173/ruleSet');

  await page.locator(".option", { hasText: "4" }).click();

  const roundsInput = page.locator('.option', { hasText: "4" }).locator('input');
  await expect(roundsInput).toBeChecked();

  await expect(page.locator(".body-btn")).toBeDisabled();
  
});

test('select rounds and 2 cat no create game ', async ({ page }) => {
  await page.goto('http://localhost:5173/ruleSet');

  const categories = ['Animals', 'Sports'];

  for (const cat of categories) {
    await page.locator('.option', { hasText: cat }).click();
  }

  for (const cat of categories) {
    await expect(
      page.locator('.option', { hasText: cat }).locator('input')
    ).toBeChecked();
  }

  await page.locator(".option", { hasText: "4" }).click();

  const roundsInput = page.locator('.option', { hasText: "4" }).locator('input');
  await expect(roundsInput).toBeChecked();

  await expect(page.locator(".body-btn")).toBeDisabled();
});

test('select all create game ', async ({ page }) => {
  await page.goto('http://localhost:5173/ruleSet');

  const categories = ['Animals', 'Fruit', 'Sports'];

  for (const cat of categories) {
    await page.locator('.option', { hasText: cat }).click();
  }

  for (const cat of categories) {
    await expect(
      page.locator('.option', { hasText: cat }).locator('input')
    ).toBeChecked();
  }

  await page.locator(".option", { hasText: "4" }).click();

  const roundsInput = page.locator('.option', { hasText: "4" }).locator('input');
  await expect(roundsInput).toBeChecked();

  await page.locator(".body-btn").click();
});



/*

commented this out for the pipeline


test('create game navigates to lobby ', async ({ page }) => {
  await page.goto('http://localhost:5173/ruleSet');

 const categories = ['Animals', 'Fruit', 'Sports'];

  for (const cat of categories) {
    await page.locator('.option', { hasText: cat }).click();
  }

  for (const cat of categories) {
    await expect(
      page.locator('.option', { hasText: cat }).locator('input')
    ).toBeChecked();
  }

  await page.locator(".option", { hasText: "4" }).click();

  const roundsInput = page.locator('.option', { hasText: "4" }).locator('input');
  await expect(roundsInput).toBeChecked();

  await page.locator(".body-btn").click();

  await expect(page).toHaveURL(/lobby/);
});*/