public class GhostPlayer
{
    protected string uid;
    public string name { get; protected set; }

    public GhostPlayer(string name, string UID)
    {
        this.name = name;
        this.UID = UID;
    }

    public string UID
    {
        get
        {
            if (uid == "") { uid = this.GetHashCode().ToString(); }
            return uid;
        }
        set
        {
            uid = value;
        }
    }
}