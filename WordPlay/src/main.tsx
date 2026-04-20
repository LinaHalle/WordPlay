console.log("main.tsx is running");
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import StartPage from './pages/StartPage';
import LobbyPage from './pages/LobbyPage';
import GamePage from './pages/GamePage';
import RuleSet from './pages/RuleSet';

const router = createBrowserRouter([
  {
    path: '/',
    element: <StartPage />
  },
  {
    path: '/ruleSet',
    element: <RuleSet />
  },
  {
    path: '/game/:gameId',
    element: <GamePage/>
  },
  {
    path: '/lobby/:gameId',
    element: <LobbyPage/>
  }
]);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
);