import { test, expect } from "@playwright/test";

import { Page, Route } from "@playwright/test";

/**
 * Helper: mock game response
 */
const mockGame = (overrides = {}) => ({
  gameId: "123",
  hostId: "1",
  rounds: 3,
  categories: ["Animal"],
  players: [],
  currentLetter: "A",
  scoreboard: {},
  status: "InRound",
  ...overrides
});


/** Helper: mock /games/123 */
async function setupGameMock(page: Page, gameData: unknown) {
  await page.route("**/games/123", (route: Route) =>
    route.fulfill({
      status: 200,
      contentType: "application/json",
      body: JSON.stringify(gameData)
    })
  );
}

/**
 * Helper: skip countdown
 */
async function skipCountdown(page: Page) {
  await page.waitForTimeout(3500);
}

test("game loads correctly", async ({ page }) => {
  await setupGameMock(page, mockGame());

  await page.goto("http://localhost:5173/game/123");

  await expect(
    page.getByRole("heading", { name: "Fill in answers" })
  ).toBeVisible();
});

test("renders categories", async ({ page }) => {
  await setupGameMock(page,
    mockGame({ categories: ["Animal", "City"] })
  );

  await page.goto("http://localhost:5173/game/123");
  await skipCountdown(page);

  await expect(page.locator("label")).toHaveCount(2);
});

test("user can fill answers", async ({ page }) => {
  await setupGameMock(page, mockGame());

  await page.goto("http://localhost:5173/game/123");
  await skipCountdown(page);

  const input = page.locator("input").first();

  await expect(input).toBeVisible();

  await input.fill("Ant");

  await expect(input).toHaveValue("Ant");
});

test("submits answers", async ({ page }) => {
  await setupGameMock(page, mockGame());

  let called = false;

  await page.route("**/games/123/answers", route => {
    called = true;
    route.fulfill({ status: 200 });
  });

  await page.goto("http://localhost:5173/game/123");
  await skipCountdown(page);

  await page.fill("input", "Ant"); // must match letter A

  const button = page.getByRole("button", { name: "DONE" });

  await expect(button).toBeEnabled();
  await button.click();

  expect(called).toBeTruthy();
});

test("scoreboard is sorted correctly", async ({ page }) => {
  await setupGameMock(page,
    mockGame({
      status: "ShowingLeaderboard",
      players: [
        { playerId: "1", userName: "Anna" },
        { playerId: "2", userName: "Bob" }
      ],
      scoreboard: {
        "1": 10,
        "2": 20
      }
    })
  );

  await page.goto("http://localhost:5173/game/123");

  const rows = page.locator(".score-row");

  await expect(rows.first()).toContainText("Bob");
});

test("shows winner", async ({ page }) => {
  await setupGameMock(page,
    mockGame({
      status: "GameEnded",
      players: [
        { playerId: "1", userName: "Anna" },
        { playerId: "2", userName: "Bob" }
      ],
      scoreboard: {
        "1": 50,
        "2": 20
      }
    })
  );

  await page.goto("http://localhost:5173/game/123");

  await expect(page.locator(".winner-text")).toContainText("Anna");
});

test("only host sees next round button", async ({ page }) => {
  await page.addInitScript(() => {
    localStorage.setItem("playerId", "1");
  });

  await setupGameMock(page,
    mockGame({
      status: "ShowingLeaderboard",
      hostId: "1"
    })
  );

  await page.goto("http://localhost:5173/game/123");

  await expect(
    page.getByRole("button", { name: "Next Round" })
  ).toBeVisible();
});

test("non-host does NOT see next round button", async ({ page }) => {
  await page.addInitScript(() => {
    localStorage.setItem("playerId", "2");
  });

  await setupGameMock(page,
    mockGame({
      status: "ShowingLeaderboard",
      hostId: "1"
    })
  );

  await page.goto("http://localhost:5173/game/123");

  await expect(
    page.getByRole("button", { name: "Next Round" })
  ).toHaveCount(0);
});