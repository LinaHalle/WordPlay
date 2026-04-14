import { defineConfig} from '@playwright/test';


/**
 * Read environment variables from file.
 * https://github.com/motdotla/dotenv
 */
// import dotenv from 'dotenv';
// import path from 'path';
// dotenv.config({ path: path.resolve(__dirname, '.env') });

/**
 * See https://playwright.dev/docs/test-configuration.
 */
export default defineConfig(
  {
    webServer: [
    {
      command: "npm run start",
      url: "http://localhost:5173",
      reuseExistingServer: !process.env.CI,
    },
    {
      command: "dotnet run",
      url: "http://localhost:5000",
      reuseExistingServer: !process.env.CI,
    }
  ]
});