using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class BoardElement
{
    protected int uid;
    protected Rectangle position;
    public virtual Rectangle Position { get { return position; } protected set { position = value; } }
    public virtual Texture2D texture { get; protected set; }

    public BoardElement(Rectangle position, Texture2D texture, int uid = 0)
    {
        this.Position = position;
        this.texture = texture;
        this.UID = uid;
    }

    public abstract void OnClick();

    public int UID
    {
        get
        {
            if (uid == 0) { uid = this.GetHashCode(); }
            return uid;
        }
        set
        {
            uid = value;
        }
    }
}