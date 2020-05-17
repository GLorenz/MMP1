// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

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