using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class MovingBoardElement : BoardElement
{
    public MovingBoardElement(string UID, Rectangle Rectangle, Texture2D texture) : base(UID, position, texture) { }

    public void MoveTo(Rectangle position)
    {
        this.position = position;
    }
}