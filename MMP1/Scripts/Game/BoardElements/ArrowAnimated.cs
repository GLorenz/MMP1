// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class ArrowAnimatable : NonMovingBoardElement, IVisibleBoardElement
{
    private static readonly float defaultDistance = 200;

    private float angle, thickness, distance;
    private Vector2 origin, scale;

    public ArrowAnimatable(Rectangle position, string UID, int zPosition) : base(position, TextureResources.Get("arrow"), UID, zPosition)
    {
        distance = defaultDistance;
        Setup();
    }

    protected void Setup()
    {
        thickness = 0.1f;
        origin = new Vector2(0, 0.5f * texture.Height);
        scale = new Vector2(distance / texture.Width / 2, thickness);
    }

    public void AnimateAngle(float angle)
    {
        this.angle = angle;
    }

    public float GetAngle()
    {
        return angle;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position.Center.ToVector2(), null, Color.White, angle, origin, scale, SpriteEffects.None, ZPosition / Board.Instance().MaxDepth);
    }

    public override void OnClick()
    {

    }
}