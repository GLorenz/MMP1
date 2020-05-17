using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System;

public class TextWrapper
{
    // code from: https://stackoverflow.com/questions/15986473/how-do-i-implement-word-wrap
    public static string Wrap(SpriteFont spriteFont, string text, int maxLineWidth)
    {
        string[] words = text.Split(' ','_');
        StringBuilder sb = new StringBuilder();
        float lineWidth = 0f;
        float spaceWidth = spriteFont.MeasureString(" ").X;

        foreach (string word in words)
        {
            Vector2 size = Vector2.Zero;
            try
            {
                size = spriteFont.MeasureString(word);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine("Cannot display question: "+ ae.Message);
                return text;
            }

            if (lineWidth + size.X < maxLineWidth)
            {
                sb.Append(word + " ");
                lineWidth += size.X + spaceWidth;
            }
            else
            {
                sb.Append("\n" + word + " ");
                lineWidth = size.X + spaceWidth;
            }
        }

        return sb.ToString();
    }
}