import Card from "../components/Card";
import Button from "../components/Button";

import "../styles/global.css";
import "../styles/theme.css";
import "../styles/startpage.css";
import "../styles/card.css";
import "../styles/button.css";

export default function StartPage() {
  return (
    <div className="startpage">
      <h1 className="title">BRAINFART</h1>

      <Card className="main-card">
        <input className="input" placeholder="ENTER NAME" />

        <Button>HOST GAME</Button>
        <Button>JOIN GAME</Button>
      </Card>

      <p className="help-text">Don't know the rules?</p>

      <Button>RULES</Button>
    </div>
  );
}



StartPage.route = {
  path: '/',
  menuLabel: 'StartPage',
  index: 1
}