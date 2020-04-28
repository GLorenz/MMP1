using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class NonMovingBoardElement : BoardElement
{
    public NonMovingBoardElement(Rectangle position, Texture2D texture, int UID = 0) : base(position, texture, UID) { }
}