using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class MovingBoardElement : BoardElement, IVisibleBoardElement
{
    public MovingBoardElement(Rectangle position, Texture2D texture, int zPosition = 0, int UID = 0) : base(position, texture, zPosition, UID) { }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        DrawDefault(spriteBatch);
    }

    public void MoveTo(Point position)
    {
        this.position.X = position.X;
        this.position.Y = position.Y;
    }

    public virtual void MoveTo(PyramidFloorBoardElement element)
    {
        MoveTo(element.Position.Location);
    }
}