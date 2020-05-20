// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using System;
using System.Collections.Generic;

public class PlayerManager : IObservable
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
    public List<IObserver> observers { get; set; }

    private PlayerManager()
    {
        ghostPlayers= new List<GhostPlayer>();
        playerMeeples = new Dictionary<GhostPlayer, List<GhostMeeple>>();
        observers = new List<IObserver>();
    }

    public bool AddMeepleRef(GhostMeeple m)
    {
        if (!playerMeeples.ContainsKey(m.ghostPlayer))
        {
            playerMeeples.Add(m.ghostPlayer, new List<GhostMeeple>());
        }

        // guaranteed to return non null
        List<GhostMeeple> selected = playerMeeples[m.ghostPlayer];

        if (selected.Count < Game1.meepleCount)
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
        NotifyObservers();
    }

    public void SetPlayer(Player p)
    {
        local = p;
    }

    public GhostPlayer GetByUID(string UID)
    {
        return ghostPlayers.Find(g => g.UID.Equals(UID));
    }

    public List<GhostPlayer> GetGhostPlayersOnly()
    {
        List<GhostPlayer> result = new List<GhostPlayer>(ghostPlayers);
        result.Remove(local);
        return result;
    }

    public void AddObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifyObservers()
    {
        foreach(IObserver observer in observers) { observer.Update(); }
    }
}