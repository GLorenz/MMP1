// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public class RemoveGhostPlayerFromManagerCommand : ICommand
{
    private GhostPlayer player;

    public RemoveGhostPlayerFromManagerCommand(GhostPlayer player)
    {
        this.player = player;
    }

    public void Execute()
    {
        PlayerManager.Instance().RemoveGhost(player);
    }
}