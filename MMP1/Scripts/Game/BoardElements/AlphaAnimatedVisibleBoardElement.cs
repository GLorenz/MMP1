using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Tasks;

public class AlphaAnimatedVisibleBoardElement : StaticVisibleBoardElement
{
    public delegate void AnimationCompleteCallback(AlphaAnimatedVisibleBoardElement element);
    protected float alpha;
    protected Color color;

    protected bool isFading;
    protected float alphaTowards;

    protected float moveSpeed = 0.07f;
    private static readonly int frameRateMS = 1000 / 60;

    public AlphaAnimatedVisibleBoardElement(Rectangle position, Texture2D texture, string UID, int zPosition = 0, float startAlpha = 1f) : base(position, texture, UID, zPosition)
    {
        color = Color.White;
        Alpha = startAlpha;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, null, color, 0, Vector2.Zero, SpriteEffects.None, ZPosition / Board.Instance().MaxDepth);
    }

    public void Fade(float alphaTowards, AnimationCompleteCallback callback)
    {
        this.alphaTowards = alphaTowards;
        if(!isFading)
        {
            new Task(() => FadeTowards(callback)).Start();
        }
    }

    protected async void FadeTowards(AnimationCompleteCallback callback)
    {
        isFading = true;

        while (Math.Abs(Alpha - alphaTowards) > 0.25f)
        {
            Alpha = alpha + moveSpeed * (alphaTowards - alpha);
            await Task.Delay(frameRateMS);
        }

        Alpha = alphaTowards;
        isFading = false;

        callback?.Invoke(this);
    }

    public float Alpha
    {
        get { return alpha; }
        set {
            alpha = value;
            color = Color.White * value;
        }
    }
}