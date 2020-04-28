public class GhostPlayer
{
    protected int uid;
    public string name { get; protected set; }

    public GhostPlayer(string name, int hash = 0)
    {
        this.name = name;
    }

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