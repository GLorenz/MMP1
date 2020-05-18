// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class TexturedBoardElement : BoardElement
{
    public virtual Texture2D texture { get; protected set; }

    public TexturedBoardElement(Rectangle position, Texture2D texture, string UID, int zPosition = 0) : base (position, UID, zPosition)
    {
        this.texture = texture;
    }

    public void DrawDefault(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, ZPosition / Board.Instance().MaxDepth);
    }
}