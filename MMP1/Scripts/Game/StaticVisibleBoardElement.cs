using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class StaticVisibleBoardElement : NonMovingBoardElement, IVisibleBoardElement
{
    public StaticVisibleBoardElement(Rectangle position, Texture2D texture, int zPosition = 0, int UID = 0) : base(position, texture, zPosition, UID)
    {
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawDefault(spriteBatch);
    }

    public override void OnClick()
    {
        
    }
}