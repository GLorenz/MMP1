// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public class AddToBoardCommand : ICommand
{
    private BoardElement[] elements;

    public AddToBoardCommand(params BoardElement[] element)
    {
        this.elements = element;
    }

    public void Execute()
    {
        Board.Instance().AddElement(elements);
    }
}