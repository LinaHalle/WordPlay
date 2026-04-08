import Card from "../components/Card";
import Button from "../components/Button";

import "../index.css";


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

      <Button className="rules-btn">RULES</Button>
    </div>
  );
}



StartPage.route = {
  path: '/',
  menuLabel: 'StartPage',
  index: 1
}