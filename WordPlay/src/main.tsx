console.log("main.tsx is running");
import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';

import StartPage from './pages/StartPage';
import RuleSet from './pages/RuleSet';
import LobbyPage from './pages/LobbyPage';

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
    path: '/lobby',
    element: <LobbyPage />
  }
]);

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <RouterProvider router={router} />
  </StrictMode>
);