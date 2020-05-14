using Microsoft.Xna.Framework;

class AddGhostMeepleToPlayerManager : ICommand
{
    private GhostMeeple meeple;

    public AddGhostMeepleToPlayerManager(GhostMeeple meeple)
    {
        this.meeple = meeple;
    }

    public void execute()
    {
        PlayerManager.Instance().AddMeepleRef(meeple);
    }
}