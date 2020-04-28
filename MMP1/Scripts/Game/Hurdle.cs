using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Hurdle : MovingBoardElement
{
    public Hurdle(Rectangle position, Texture2D texture, int UID = 0) : base(position, texture, UID) { }

    public override void OnClick()
    {
        StartPuzzle();
    }

    public void StartPuzzle()
    {

    }
}