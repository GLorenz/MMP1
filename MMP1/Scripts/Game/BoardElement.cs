using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class BoardElement
{
    public virtual string UID { get; protected set; }
    public virtual Rectangle position { get; protected set; }
    public virtual Texture2D texture { get; protected set; }

    public BoardElement(string UID, Rectangle position, Texture2D texture)
    {
        this.UID = UID;
        this.position = position;
        this.texture = texture;
    }

    public abstract void OnClick();
}