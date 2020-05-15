using Microsoft.Xna.Framework;

public abstract class BoardElement
{
    protected string uid;
    protected Rectangle position;
    protected int zPosition;

    public virtual Rectangle Position { get { return position; } protected set { position = value; } }
    public virtual int ZPosition { get { return zPosition; } protected set { zPosition = value; } }

    public BoardElement(Rectangle position, string UID, int zPosition = 0)
    {
        this.Position = position;
        this.uid = UID;
        this.ZPosition = zPosition;
    }

    public abstract void OnClick();

    public string UID
    {
        get
        {
            if (uid.Length == 0) { uid = this.GetHashCode().ToString(); }
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
}