public class RemoveGhostMeepleFromManagerCommand : ICommand
{
    private GhostMeeple meeple;

    public RemoveGhostMeepleFromManagerCommand(GhostMeeple meeple)
    {
        this.meeple = meeple;
    }

    public void Execute()
    {
        PlayerManager.Instance().RemoveMeepleRef(meeple);
    }
}