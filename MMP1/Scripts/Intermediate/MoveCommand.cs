using Microsoft.Xna.Framework;

public class MoveCommand : ICommand
{
    MovingBoardElement element;
    Vector2 moveTo;

    public MoveCommand(MovingBoardElement element, Vector2 moveTo)
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
}