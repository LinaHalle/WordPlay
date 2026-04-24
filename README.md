CI-pipeline
Översikt
Projektet har en GitHub Actions-pipeline som bygger och testar både frontend (React + Vite) och backend (ASP.NET Core), samt kör end-to-end-tester med Playwright och API-integrationstester med Postman/Newman. Pipelinen är definierad i .github/workflows/main.yml och består av fyra jobb som körs delvis parallellt, delvis sekventiellt.

När pipelinen körs
Vid push till main-branchen
Vid alla pull requests (oavsett målbranch)
Jobbflöde
frontend  ─┐
           ├─► playwright ─► api-tests
backend   ─┘
frontend och backend körs parallellt.
playwright väntar tills båda är klara innan det startar.
api-tests körs sist, efter att playwright lyckats.
Om något jobb misslyckas stoppas de efterföljande.

Jobben
frontend
Bygger och testar React-klienten. Sätter upp Node 20, installerar npm-beroenden, kör npm run build (TypeScript-check + Vite-bygge) och slutligen npm test. Säkerställer att klienten kompilerar och att frontendtesterna passerar.

backend
Bygger och testar .NET-API:et. Sätter upp .NET 8, kör dotnet restore och dotnet build, och kör sedan xUnit-testerna i word.Tests/Brainfart.Tests/. Säkerställer att backend-koden kompilerar och att enhetstesterna passerar.

playwright
Kör end-to-end-tester i webbläsaren. Installerar Chromium och kör testerna i WordPlay/tests/ med npx playwright test. Verifierar att hela appen (frontend + backend tillsammans) fungerar ur ett användarperspektiv.

api-tests
Startar backend lokalt på port 5095, väntar in att servern svarar (pollar upp till 60 sekunder) och kör sedan nio Postman-collections via Newman:

CreateLobby
JoinGame
ChangeSettings
StartGame
SubmitAnswers
FinishRound
GetGameState
FullGameFlow
EdgeCases
Detta är en integrationskontroll av varje API-endpoint samt ett helt spelflöde.

Verktyg
Verktyg	Version	Används i
Node.js	20	frontend, playwright
.NET SDK	8.0	backend, api-tests
Playwright	(från npm)	playwright (endast Chromium)
Newman	senaste	api-tests
Alla jobb körs på ubuntu-latest.
