import Card from "../components/Card";
import Button from "../components/Button";

import "../index.css";

export default function LobbyPage() {
  return (
    <div className="page-center">
  <h1 className="title" >LOBBY</h1>

  <div className="cards-container">
    <div className="card-section">
      <p>Number of rounds:</p>

      <Card className="long-card">
  {[3,4,5,6,7,8,9,10].map(num => (
    <label className="option" key={num}>
      <span>{num}</span>
      <input type="radio" name="rounds" />
    </label>
  ))}
</Card>
    </div>

    <div className="card-section">
      <p>Choose categories:</p>

      <Card className="long-card">
  {["Animals", "Fruit", "Sports", "Name", "Country", "City", "CarBrand", "Movies"].map(cat => (
    <label className="option" key={cat}>
      <span>{cat}</span>
      <input type="checkbox" />
    </label>
  ))}
</Card>
    </div>
  </div>

  <Button className="body-btn">Create Game</Button>
</div>

  );
}



LobbyPage.route = {
  path: '/lobby',
  menuLabel: 'LobbyPage',
  index: 2
}