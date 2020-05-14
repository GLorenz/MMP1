public class QuestionBoardElement : MovingBoardElement
{
    public QuestionBoardElement(PyramidFloorBoardElement below, string UID) 
        : base(below.Position, TextureResources.Get("QM"), UID, below.ZPosition+1)
    {

    }

    public override void OnClick()
    {
        
    }

    public override void MoveToLocalOnly(PyramidFloorBoardElement element)
    {
        base.MoveToLocalOnly(element);
        SetZPosition(element.ZPosition + 1);
    }

    public override void MoveTo(PyramidFloorBoardElement element)
    {
        MoveQBECommand cmd = new MoveQBECommand(this, element);
        PlayerManager.Instance().local.HandleInput(cmd, true);
    }

    /*protected override void ShareMoveTo(PyramidFloorBoardElement element)
    {
        MoveQBECommand cmd = new MoveQBECommand(this, element);
        PlayerManager.Instance().local.OnlyShare(cmd);
    }*/

    public void SetZPosition(int z)
    {
        ZPosition = z;
        CommandQueue.Queue(new RestructureBoardElementsCommand());
        //Board.Instance().ResturctureElements();
    }
}