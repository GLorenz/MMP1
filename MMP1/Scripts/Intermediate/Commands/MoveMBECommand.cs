using System;

public class MoveMBECommand : INetworkCommand
{
    public const string name = "MV_MBE";

    protected MovingBoardElement element;
    protected PyramidFloorBoardElement moveTo;

    public MoveMBECommand(MovingBoardElement element, PyramidFloorBoardElement moveTo)
    {
        this.element = element;
        this.moveTo = moveTo;
    }

    public void execute()
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

    public static MoveMBECommand FromInput(SerializableCommand sCommand)
    {
        try
        {
            MovingBoardElement movingEl = (MovingBoardElement)Board.Instance().FindByUID(sCommand.UID);
            PyramidFloorBoardElement floorEl = (PyramidFloorBoardElement)Board.Instance().FindByUID(sCommand.body);

            return new MoveMBECommand(movingEl, floorEl);
        }
        catch (InvalidCastException ice)
        {
            Console.WriteLine(ice.Message);
            return null;
        }
    }
    
    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand(name, element.UID, moveTo.UID, shouldShare);
    }
}