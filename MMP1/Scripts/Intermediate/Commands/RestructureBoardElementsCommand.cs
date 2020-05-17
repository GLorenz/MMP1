// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public class RestructureBoardElementsCommand : ICommand
{
    public void Execute()
    {
        Board.Instance().ResturctureElements();
    }
}