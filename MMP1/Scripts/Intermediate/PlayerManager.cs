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

    public Dictionary<GhostPlayer, List<GhostMeeple>> playerMeeples { get; }

    private PlayerManager()
    {
        ghostPlayers= new List<GhostPlayer>();
        playerMeeples = new Dictionary<GhostPlayer, List<GhostMeeple>>();
    }

    public bool AddMeepleRef(GhostMeeple m)
    {
        //todo: ghost player is null -> crash
        if (!playerMeeples.ContainsKey(m.ghostPlayer))
        {
            playerMeeples.Add(m.ghostPlayer, new List<GhostMeeple>());
        }

        // guaranteed to return non null
        List<GhostMeeple> selected = playerMeeples[m.ghostPlayer];

        if(selected.Count < Game1.meepleCount)
        {
            selected.Add(m);
            return true;
        }
        return false;
    }

    public List<GhostMeeple> GetLocalMeeples()
    {
        return playerMeeples[local];
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