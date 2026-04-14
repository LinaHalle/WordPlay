import { defineConfig } from "@playwright/test";


export default defineConfig({
    webServer: {
    command: "npm run dev",
    url: "http://localhost:5173/",
    reuseExistingServer: !process.env.CI,
    },
});
