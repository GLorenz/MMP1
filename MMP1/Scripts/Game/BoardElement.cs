using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class BoardElement
{
    protected int uid;
    protected Rectangle position;
    protected int zPosition;

    public virtual Rectangle Position { get { return position; } protected set { position = value; } }
    public virtual Texture2D texture { get; protected set; }
    public virtual int ZPosition { get { return zPosition; } protected set { zPosition = value; } }

    public BoardElement(Rectangle position, Texture2D texture, int zPosition = 0, int uid = 0)
    {
        this.Position = position;
        this.texture = texture;
        this.UID = uid;
        this.ZPosition = zPosition;
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

    public static int CompareByZPosition(BoardElement b1, BoardElement b2)
    {
        return b1.ZPosition.CompareTo(b2.ZPosition);
    }

    public void DrawDefault(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, ZPosition / Board.Instance().MaxDepth);
    }
}