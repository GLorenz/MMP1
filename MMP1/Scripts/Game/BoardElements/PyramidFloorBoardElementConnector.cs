// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class PyramidFloorBoardElementConnector : NonMovingBoardElement, IVisibleBoardElement
{
    protected Vector2 to, from;
    protected Vector2 direction, origin, scale;
    protected float distance, angle, thickness;

    protected PyramidFloorBoardElement fromBE, toBE;

    public PyramidFloorBoardElementConnector(PyramidFloorBoardElement from, PyramidFloorBoardElement to, string UID, int zPosition = 0) : 
        base(
            from.Position,
            TextureResources.Get("PyramidFieldConnectionShort"),
            UID,
            zPosition
        )
    {
        this.fromBE = from;
        this.toBE = to;

        Setup();
    }

    protected virtual void Setup()
    {
        thickness = 0.3f;
        origin = new Vector2(0, thickness * texture.Width);

        this.to = toBE.Position.Center.ToVector2() + origin;
        this.from = fromBE.Position.Center.ToVector2() + origin;

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