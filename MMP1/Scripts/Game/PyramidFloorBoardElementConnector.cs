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
        /*new Rectangle(
                ((to.Position.Location - from.Position.Location).ToVector2() / 2f).ToPoint(),
                new Point((int)(to.Position.Location - from.Position.Location).ToVector2().Length(), (int)(to.Position.Location - from.Position.Location).ToVector2().Length())
            )*/
        this.to = to.Position.Center.ToVector2();
        this.from = from.Position.Center.ToVector2();

        Setup();
    }

    private void Setup()
    {
        direction = to - from;
        direction.Normalize();

        distance = (to - from).Length();
        //angle = (float)Math.Atan2(to.Y - from.Y, to.X - from.X);
        angle = (float)Math.Atan2(direction.Y,direction.X);
        thickness = 0.3f;

        origin = new Vector2(0, thickness / 2f);
        scale = new Vector2(distance / texture.Width, thickness);
        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        //DrawDefault(spriteBatch);
        spriteBatch.Draw(texture, position.Center.ToVector2(), null, Color.White, angle, origin, scale, SpriteEffects.None, ZPosition / Board.Instance().MaxDepth);
        //spriteBatch.Draw(texture, position, null, Color.White, angle, Vector2.Zero, SpriteEffects.None, ZPosition / Board.Instance().MaxDepth);
    }


    public override void OnClick()
    {
        
    }
}