using System.Collections.Generic;

public class PlayerManager
{
    private static PlayerManager manager;
    public static PlayerManager Instance()
    {
        if (manager == null) { manager = new PlayerManager(); }
        return manager;
    }

    public Player local { get; private set; }
    public List<GhostPlayer> ghostPlayers{ get; private set; }

    private PlayerManager()
    {
        ghostPlayers= new List<GhostPlayer>();
    }

    public void AddGhost(GhostPlayer ghost)
    {
        ghostPlayers.Add(ghost);
    }

    public void SetLocal(Player p)
    {
        local = p;
    }

    public GhostPlayer GetByUID(int UID)
    {
        return ghostPlayers.Find(g => g.UID.Equals(UID));
    }
}