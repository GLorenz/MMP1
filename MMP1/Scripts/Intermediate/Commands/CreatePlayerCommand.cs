// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public class CreatePlayerCommand : ICommand
{
    private string playerName;
    private string playerUID;

    public CreatePlayerCommand(string name, string uid)
    {
        playerName = name;
        playerUID = uid;
    }

    public void Execute()
    {
        Player player = new Player(playerName, playerUID);
        PlayerManager.Instance().SetPlayer(player);
        player.ConnectClient();
        player.Create();
    }
}