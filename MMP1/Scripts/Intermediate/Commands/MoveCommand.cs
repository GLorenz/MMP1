using Microsoft.Xna.Framework;
using System;

public class MoveCommand : INetworkCommand
{
    public const string name = "MV";
    MovingBoardElement element;
    Point moveTo;

    public MoveCommand(MovingBoardElement element, Point moveTo)
    {
        this.element = element;
        this.moveTo = moveTo;
    }

    public void execute()
    {
        element.MoveTo(moveTo);
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        Point relativeMoveTo = UnitConvert.ToScreenRelative(moveTo);
        return new SerializableCommand(name, element.UID, relativeMoveTo.X + ";" + relativeMoveTo.Y, shouldShare);
    }

    public static MoveCommand FromInput(SerializableCommand sCommand)
    {
        try
        {
            MovingBoardElement movingEl = (MovingBoardElement)Board.Instance().FindByUID(sCommand.UID);
            string[] positions = sCommand.body.Split(';');
            Point moveTo = UnitConvert.ToAbsolute(new Point(int.Parse(positions[0]), int.Parse(positions[1])));

            return new MoveCommand(movingEl, moveTo);
        }
        catch (InvalidCastException ice)
        {
            Console.WriteLine(ice.Message);
            return null;
        }
    }
}