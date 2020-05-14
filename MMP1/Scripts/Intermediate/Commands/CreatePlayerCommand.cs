public class CreatePlayerCommand : ICommand
{
    private string playerName;
    private string playerUID;

    public CreatePlayerCommand(string name, string uid)
    {
        playerName = name;
        playerUID = uid;
    }

    public void execute()
    {
        Player player = new Player(playerName, playerUID);
        PlayerManager.Instance().SetPlayer(player);
        player.ConnectClient();
        player.Create();
    }
}