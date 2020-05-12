public class QuestionBoardElement : MovingBoardElement
{
    public QuestionBoardElement(PyramidFloorBoardElement below, string UID) 
        : base(below.Position, TextureResources.Get("QM"), UID, below.ZPosition+1)
    {

    }

    public override void OnClick()
    {
        
    }

    public override void MoveTo(PyramidFloorBoardElement element)
    {
        base.MoveTo(element);
        SetZPosition(element.ZPosition + 1);
    }

    public void SetZPosition(int z)
    {
        ZPosition = z;
        Board.Instance().ResturctureElements();
    }
}