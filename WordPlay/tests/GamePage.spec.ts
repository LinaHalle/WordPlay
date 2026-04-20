import { test, expect } from "@playwright/test";

test('game loads correctly', async ({ page }) => {
  await page.route('**/games/123', route =>
    route.fulfill({
      status: 200,
      body: JSON.stringify({
        gameId: "123",
        hostId: "1",
        rounds: 3,
        categories: ["Animal"],
        players: [],
        currentLetter: "A",
        scoreboard: {},
        status: 1
      })
    })
  );

  await page.goto('http://localhost:5173/game/123');

  await expect(page.locator('h1')).toBeVisible();
});

test('renders categories', async ({ page }) => {
  await page.route('**/games/123', route =>
    route.fulfill({
      status: 200,
      body: JSON.stringify({
        gameId: "123",
        hostId: "1",
        rounds: 3,
        categories: ["Animal", "City"],
        players: [],
        currentLetter: "A",
        scoreboard: {},
        status: 1
      })
    })
  );

  await page.goto('http://localhost:5173/game/123');

  await expect(page.locator('label')).toHaveCount(2);
});

test('user can fill answers', async ({ page }) => {
  await page.route('**/games/123', route =>
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        gameId: "123",
        hostId: "1",
        rounds: 3,
        categories: ["Animal"],
        players: [],
        currentLetter: "A",
        scoreboard: {},
        status: 1
      })
    })
  );

  await page.goto('http://localhost:5173/game/123');

  await expect(page.locator('input').first()).toBeVisible();

  await page.locator('input').first().fill('Cat');

  await expect(page.locator('input').first()).toHaveValue('Cat');
});

test('submits answers', async ({ page }) => {
  await page.route('**/games/123', route =>
    route.fulfill({
      status: 200,
      contentType: 'application/json',
      body: JSON.stringify({
        gameId: "123",
        hostId: "1",
        rounds: 3,
        categories: ["Animal"],
        players: [],
        currentLetter: "A",
        scoreboard: {},
        status: 1
      })
    })
  );

  let called = false;

  await page.route('**/answers', route => {
    called = true;
    route.fulfill({ status: 200 });
  });

  await page.goto('http://localhost:5173/game/123');

  await page.click('text=DONE');

  expect(called).toBeTruthy();
});

test('scoreboard is sorted correctly', async ({ page }) => {
  await page.route('**/games/123', route =>
    route.fulfill({
      status: 200,
      body: JSON.stringify({
        gameId: "123",
        hostId: "1",
        rounds: 3,
        categories: [],
        players: [
          { playerId: "1", userName: "Anna" },
          { playerId: "2", userName: "Bob" }
        ],
        currentLetter: "A",
        scoreboard: {
          "1": 10,
          "2": 20
        },
        status: 2
      })
    })
  );

  await page.goto('http://localhost:5173/game/123');

  const rows = page.locator('.score-row');

  await expect(rows.first()).toContainText('Bob');
});

test('shows winner', async ({ page }) => {
  await page.route('**/games/123', route =>
    route.fulfill({
      status: 200,
      body: JSON.stringify({
        gameId: "123",
        hostId: "1",
        rounds: 3,
        categories: [],
        players: [
          { playerId: "1", userName: "Anna" },
          { playerId: "2", userName: "Bob" }
        ],
        currentLetter: "A",
        scoreboard: {
          "1": 50,
          "2": 20
        },
        status: 3
      })
    })
  );

  await page.goto('http://localhost:5173/game/123');

  await expect(page.locator('.winner-text')).toContainText('Anna');
});


test('only host sees next round button', async ({ page }) => {
  await page.addInitScript(() => {
    localStorage.setItem('playerId', '1');
  });

  await page.route('**/games/123', route =>
    route.fulfill({
      status: 200,
      body: JSON.stringify({
        hostId: "1",
        players: [],
        categories: [],
        scoreboard: {},
        status: 2
      })
    })
  );

  await page.goto('http://localhost:5173/game/123');

  await expect(page.locator('text=Next Round')).toBeVisible();
});