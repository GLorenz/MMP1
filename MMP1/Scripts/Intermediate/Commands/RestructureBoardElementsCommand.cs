public class RestructureBoardElementsCommand : ICommand
{
    public void Execute()
    {
        Board.Instance().ResturctureElements();
    }
}