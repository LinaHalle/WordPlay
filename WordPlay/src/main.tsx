console.log("main.tsx is running");
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import StartPage from './pages/StartPage';
import LobbyPage from './pages/LobbyPage';
<<<<<<< HEAD
import RuleSet from './pages/RuleSet';
=======
import GamePage from './pages/GamePage';
>>>>>>> origin/gamepage

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
<<<<<<< HEAD
    path: '/lobby/:gameId',
    element: <LobbyPage />
=======
    path: '/game/:gameId',
    element: <GamePage/>
>>>>>>> origin/gamepage
  }
]);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
);