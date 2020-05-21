// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

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

    public override void MoveToAndShare(PyramidFloorBoardElement element)
    {
        MoveQBECommand cmd = new MoveQBECommand(this, element);
        PlayerManager.Instance().local.HandleInput(cmd, true);
    }

    public void SetZPosition(int z)
    {
        ZPosition = z;
        CommandQueue.Queue(new RestructureBoardElementsCommand());
    }
}