# Instructions

- Following Playwright test failed.
- Explain why, be concise, respect Playwright best practices.
- Provide a snippet of code with the fix, if possible.

# Test info

- Name: .features-gen/ui/e2e/ui/features/smoke.feature.spec.js >> Smoke >> Startsidan går att öppna
- Location: .features-gen/ui/e2e/ui/features/smoke.feature.spec.js:6:3

# Error details

```
Error: Expected title to include "MinApplikation" but got "Page not found · GitHub Pages"
```

# Page snapshot

```yaml
- generic [ref=e2]:
  - heading "404" [level=1] [ref=e3]
  - paragraph [ref=e4]:
    - strong [ref=e5]: File not found
  - paragraph [ref=e6]: The site configured at this address does not contain the requested file.
  - paragraph [ref=e7]:
    - text: If this is your site, make sure that the filename case matches the URL as well as any file permissions.
    - text: For root URLs (like
    - code [ref=e8]: http://example.com/
    - text: ) you must provide an
    - code [ref=e9]: index.html
    - text: file.
  - paragraph [ref=e10]:
    - link "Read the full documentation" [ref=e11] [cursor=pointer]:
      - /url: https://help.github.com/pages/
    - text: for more information about using
    - strong [ref=e12]: GitHub Pages
    - text: .
  - generic [ref=e13]:
    - link "GitHub Status" [ref=e14] [cursor=pointer]:
      - /url: https://githubstatus.com
    - text: —
    - link "@githubstatus" [ref=e15] [cursor=pointer]:
      - /url: https://twitter.com/githubstatus
  - link [ref=e16] [cursor=pointer]:
    - /url: /
```

# Test source

```ts
  1  | import { createBdd } from 'playwright-bdd';
  2  | const { Given, When, Then } = createBdd();
  3  | 
  4  | Given('att jag öppnar startsidan', async ({ page }) => {
  5  |     await page.goto('/');
  6  | });
  7  | 
  8  | Then('ska jag se sidans titel innehåller {string}', async ({ page }, expected) => {
  9  |     const title = await page.title();
  10 |     if (!title.includes(expected)) {
> 11 |         throw new Error(`Expected title to include "${expected}" but got "${title}"`);
     |               ^ Error: Expected title to include "MinApplikation" but got "Page not found · GitHub Pages"
  12 |     }
  13 | });
```