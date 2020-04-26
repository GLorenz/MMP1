using Microsoft.Xna.Framework;

public class MoveCommand : IInputCommand
{
    private const string name = "Move";
    MovingBoardElement element;
    Point moveTo;

    public MoveCommand(MovingBoardElement element, Point moveTo)
    {
        this.element = element;
        this.moveTo = moveTo;

        this.subject = element;
    }
    public object subject { get; set; }

    public void execute()
    {
        element.MoveTo(moveTo);
    }

    public Input ToInput(bool shouldShare)
    {
        return new Input(name, element.UID, moveTo.X + "," + moveTo.Y, shouldShare);
    }
}