public class AddGhostPlayerToPlayerManager : ICommand
{
    private GhostPlayer player;

    public AddGhostPlayerToPlayerManager(GhostPlayer player)
    {
        this.player = player;
    }

    public void execute()
    {
        PlayerManager.Instance().AddGhost(player);
    }
}