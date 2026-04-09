namespace Brainfart.Tests;

public class CreateGameTest
{

    private GameService _service = new GameService();
    [Fact]
    public void CreateGameTest()
    {
        // arrange
        _service.hostName("alice");
        _service.categories["Cars", "Cities", "movies"];


        //act
        _service.CreateGame(hostName, categories);


        //assert
        Assert.Equal(gameId(""));
        Assert.Equal(HostName(""));

    }
}
