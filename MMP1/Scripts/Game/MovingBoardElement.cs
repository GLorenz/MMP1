using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class MovingBoardElement : BoardElement
{
    public MovingBoardElement(string UID, Rectangle position, Texture2D texture) : base(UID, position, texture) { }

    public void MoveTo(Point position)
    {
        this.position.X = position.X;
    }
}