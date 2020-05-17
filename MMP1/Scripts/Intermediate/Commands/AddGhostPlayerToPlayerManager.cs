public class AddGhostPlayerToPlayerManager : ICommand
{
    private GhostPlayer player;

    public AddGhostPlayerToPlayerManager(GhostPlayer player)
    {
        this.player = player;
    }

    public void Execute()
    {
        PlayerManager.Instance().AddGhost(player);
    }
}