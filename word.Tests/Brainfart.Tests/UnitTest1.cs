namespace Brainfart.Tests;

public class CreateGameTest
{
    [Fact]
    public void CreateGameTest()
    {
        //arrange
        GameService service = new GameService();
        var hostName = "Alice";
        //act 
        var (gameId, playerId, error) = service.CreateGame(hostName);
        //assert
        Assert.Null(error);
        Assert.NotEqual(playerId, gameId);
    }
}
