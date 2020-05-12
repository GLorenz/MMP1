
using Microsoft.Xna.Framework;
using System;

public class ArrowAnimatable : PyramidFloorBoardElementConnector
{
    Vector2 originalScale;

    public ArrowAnimatable(PyramidFloorBoardElement from, PyramidFloorBoardElement to, string UID, int zPosition) : base(from, to, UID, zPosition)
    {
        texture = TextureResources.Get("arrow");
    }

    protected override void Setup()
    {
        thickness = 0.1f;
        origin = new Vector2(0, 0.5f * texture.Height);
        Vector2 offset = Vector2.Zero;// new Vector2(Board.Instance().ToAbsolute(0.5f));

        this.to = toBE.Position.Center.ToVector2();
        this.from = fromBE.Position.Center.ToVector2();

        direction = to - from;
        direction.Normalize();

        distance = (to - from).Length();
        angle = (float)Math.Atan2(direction.Y, direction.X);

        scale = new Vector2(distance / texture.Width / 2, thickness);
        originalScale = new Vector2(scale.X, scale.Y);
    }

    public void Animate(float range)
    {
        scale.X = originalScale.X * (range*0.2f+0.5f);
    }
}