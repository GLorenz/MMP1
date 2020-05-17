// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public class RemoveFromBoardCommand : ICommand
{
    private BoardElement[] elements;

    public RemoveFromBoardCommand(params BoardElement[] element)
    {
        this.elements = element;
    }

    public void Execute()
    {
        Board.Instance().RemoveElement(elements);
    }
}