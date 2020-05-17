// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework.Graphics;

public interface IVisibleBoardElement
{
    void Draw(SpriteBatch spriteBatch);
    int ZPosition { get; }
}