using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class NonMovingBoardElement : BoardElement
{
    public NonMovingBoardElement(string UID, Rectangle position, Texture2D texture) : base(UID, position, texture) { }
}