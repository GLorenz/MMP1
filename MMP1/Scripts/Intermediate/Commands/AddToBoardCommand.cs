public class AddToBoardCommand : ICommand
{
    private BoardElement[] elements;

    public AddToBoardCommand(params BoardElement[] element)
    {
        this.elements = element;
    }

    public void execute()
    {
        Board.Instance().AddElement(elements);
    }
}