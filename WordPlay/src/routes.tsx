//page components
import StartPage from './pages/StartPage';
import RuleSet from './pages/RuleSet';
import LobbyPage from './pages/LobbyPage';




export default [
  {
    index: true,
    element: <StartPage />
  },
  {
    path: '/lobby',
    element: <LobbyPage />
  },
  {
    path: '/ruleset',
    element: <RuleSet />
  }
];