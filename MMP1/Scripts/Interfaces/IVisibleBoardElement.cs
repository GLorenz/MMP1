using Microsoft.Xna.Framework.Graphics;

public interface IVisibleBoardElement
{
    void Draw(SpriteBatch spriteBatch);
    int ZPosition { get; }
}