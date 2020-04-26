using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class BoardElement
{
    public virtual string UID { get; protected set; }
    protected Rectangle position;
    public virtual Rectangle Position { get { return position; } protected set { position = value; } }
    public virtual Texture2D texture { get; protected set; }

    public BoardElement(string UID, Rectangle position, Texture2D texture)
    {
        this.UID = UID;
        this.Position = position;
        this.texture = texture;
    }

    public abstract void OnClick();
}