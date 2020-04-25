using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Hurdle : MovingBoardElement
{
    public Hurdle(string UID, Rectangle position, Texture2D texture) : base(UID, position, texture) { }

    public override void OnClick()
    {
        StartPuzzle();
    }

    public void StartPuzzle()
    {

    }
}