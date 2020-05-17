using System;

public class MoveGMCommand : INetworkCommand
{
    public const string name = "MV_GM";
    
    protected GhostMeeple element;
    protected PyramidFloorBoardElement moveTo;

    public MoveGMCommand(GhostMeeple element, PyramidFloorBoardElement moveTo)
    {
        this.element = element;
        this.moveTo = moveTo;
    }

    public static MoveGMCommand FromInput(SerializableCommand sCommand)
    {
        try
        {
            GhostMeeple movingEl = (GhostMeeple)Board.Instance().FindByUID(sCommand.UID);
            PyramidFloorBoardElement floorEl = (PyramidFloorBoardElement)Board.Instance().FindByUID(sCommand.body);

            return new MoveGMCommand(movingEl, floorEl);
        }
        catch (InvalidCastException ice)
        {
            Console.WriteLine(ice.Message);
            return null;
        }
    }

    public virtual void Execute()
    {
        if (element != null && moveTo != null)
        {
            element.MoveToLocalOnly(moveTo);
        }
        else
        {
            Console.WriteLine("why is my element null ?!");
        }
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand(name, element.UID, moveTo.UID, shouldShare);
    }
}