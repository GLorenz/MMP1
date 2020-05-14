using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class MovingBoardElement : BoardElement, IVisibleBoardElement
{
    public MovingBoardElement(Rectangle position, Texture2D texture, string UID, int zPosition = 0) : base(position, texture, UID, zPosition) { }

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
        MoveMBECommand cmd = new MoveMBECommand(this, element);
        PlayerManager.Instance().local.HandleInput(cmd, true);
        /*MoveToLocalOnly(element);
        ShareMoveTo(element);*/
    }

    public virtual void MoveToLocalOnly(PyramidFloorBoardElement element)
    {
        MoveTo(element.Position.Location);
    }

    /*protected virtual void ShareMoveTo(PyramidFloorBoardElement element)
    {
        MoveMBECommand cmd = new MoveMBECommand(this, element);
        PlayerManager.Instance().local.OnlyShare(cmd);
    }*/
}