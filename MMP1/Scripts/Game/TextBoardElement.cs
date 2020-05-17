using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class TextBoardElement : BoardElement, IVisibleBoardElement
{
    public enum Alignment { Middle, LeftMiddle, LeftTop, LeftBottom }
    private string text;
    public SpriteFont font { get; protected set; }
    private Color color;
    private Vector2 textPosition;
    private Alignment alignment;

    public TextBoardElement(Rectangle position, string text, SpriteFont font, string UID, Color color, int zPosition = 0, Alignment alignment = Alignment.Middle) : base(position, UID, zPosition)
    {
        this.font = font;
        this.color = color;
        this.alignment = alignment;
        this.Text = text;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(font, text, textPosition, color);
    }

    public override void OnClick()
    {
        //nothing
    }

    public string Text
    {
        get { return text; }
        set
        {
            text = TextWrapper.Wrap(font, value, position.Width);
            textPosition = position.Location.ToVector2();

            if (alignment.Equals(Alignment.Middle))
                textPosition += ((position.Size.ToVector2() - font.MeasureString(text)) * 0.5f);
            else if(alignment.Equals(Alignment.LeftMiddle))
                textPosition.Y += ((position.Size.ToVector2().Y - font.MeasureString(text).Y) * 0.5f);
            else if (alignment.Equals(Alignment.LeftBottom))
                textPosition.Y += position.Size.ToVector2().Y - font.MeasureString(text).Y;
        }
    }
}