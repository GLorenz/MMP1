using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class QuestionBoardElement : MovingBoardElement
{
    public QuestionBoardElement(PyramidFloorBoardElement below, int UID = 0) 
        : base(below.Position, TextureResources.Get("QM"), below.ZPosition+1, UID)
    {

    }

    public override void OnClick()
    {
        
    }
}