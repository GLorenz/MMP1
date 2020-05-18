// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class NonMovingBoardElement : TexturedBoardElement
{
    public NonMovingBoardElement(Rectangle position, Texture2D texture, string UID, int zPosition = 0) : base(position, texture, UID, zPosition) { }
}