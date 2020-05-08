using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class PyramidFloorBoardElementConnector : NonMovingBoardElement, IVisibleBoardElement
{
    private Vector2 to, from;
    private Vector2 direction, origin, scale;
    private float distance, angle, thickness;

    public PyramidFloorBoardElementConnector(PyramidFloorBoardElement from, PyramidFloorBoardElement to, int zPosition, int UID = 0) : 
        base(
            from.Position,
            TextureResources.Get("PyramidFieldConnectionShort"),
            zPosition,
            UID
        )
    {
        thickness = 0.3f;
        origin = new Vector2(0, thickness * texture.Width);

        this.to = to.Position.Center.ToVector2() + origin;
        this.from = from.Position.Center.ToVector2() + origin;

        Setup();
    }

    private void Setup()
    {
        direction = to - from;
        direction.Normalize();

        distance = (to - from).Length();
        angle = (float)Math.Atan2(direction.Y,direction.X);
        
        scale = new Vector2(distance / texture.Width, thickness);
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position.Center.ToVector2(), null, Color.White, angle, origin, scale, SpriteEffects.None, ZPosition / Board.Instance().MaxDepth);
    }


    public override void OnClick()
    {
        
    }
}