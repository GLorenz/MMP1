public class RemoveFromBoardCommand : ICommand
{
    private BoardElement[] elements;

    public RemoveFromBoardCommand(params BoardElement[] element)
    {
        this.elements = element;
    }

    public void execute()
    {
        Board.Instance().RemoveElement(elements);
    }
}