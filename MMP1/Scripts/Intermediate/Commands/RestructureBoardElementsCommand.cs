public class RestructureBoardElementsCommand : ICommand
{
    public void execute()
    {
        Board.Instance().ResturctureElements();
    }
}