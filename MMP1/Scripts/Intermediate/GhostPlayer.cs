using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

public class GhostPlayer
{
    protected string uid;
    public string name { get; protected set; }
    private MeepleColor color;

    public GhostPlayer(string name, string UID = "")
    {
        this.name = name;
        this.UID = UID;
        CommandQueue.Queue(new AddGhostPlayerToPlayerManager(this));
        Console.WriteLine("added player to manager");
    }

    public virtual void Create()
    {
        Console.WriteLine("created ghost player "+UID);
        CreateMeeples();
    }

    protected virtual void CreateMeeples()
    {
        for (int i = 0; i < 2; i++)
        {
            GhostMeeple newMeep = new GhostMeeple(this, new Rectangle(0, 0, Board.Instance().boardUnit, Board.Instance().boardUnit), UID + "_meeple" + i, meepIdx:i);
            newMeep.Create();
        }
    }

    public MeepleColor MeepleColor
    {
        get
        {
            return color;
        }
        set
        {
            if (PlayerManager.Instance().playerMeeples.ContainsKey(this))
            {
                foreach (GhostMeeple gm in PlayerManager.Instance().playerMeeples[this]) { gm.Color = value; }
            }
            color = value;
        }
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