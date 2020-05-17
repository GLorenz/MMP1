// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class MovingBoardElement : TexturedBoardElement, IVisibleBoardElement
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
    }

    public virtual void MoveToLocalOnly(PyramidFloorBoardElement element)
    {
        MoveTo(element.Position.Location);
    }
}