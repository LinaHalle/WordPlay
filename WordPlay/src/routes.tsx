import type { JSX } from 'react';
import { createElement } from 'react';
//page components
import StartPage from './pages/StartPage';
import LobbyPage from './pages/LobbyPage';




export default [
  {
    index: true,
    element: <StartPage />
  },
  {
    path: '/lobby',
    element: <LobbyPage />
  }
];