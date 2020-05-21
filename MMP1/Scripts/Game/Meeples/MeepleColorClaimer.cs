// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using System;
using System.Collections.Generic;
using System.Threading;

public class MeepleColorClaimer : IObservable
{
    private List<MeepleColor> available;
    public List<IObserver> observers { get; set; }

    private MeepleColorClaimer()
    {
        observers = new List<IObserver>();
        available = new List<MeepleColor>() { MeepleColor.Black, MeepleColor.Green, MeepleColor.Red, MeepleColor.White };
    }

    public MeepleColor Next()
    {
        if(available.Count > 0)
        {
            int idx = new Random().Next(available.Count);
            return available[idx];
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// When color is sure to be free.
    /// Only issued by lobby host.
    /// Received by anyone
    /// </summary> 
    public void ClaimColor(string playerUID, MeepleColor color)
    {
        Console.WriteLine("claiming {0} for {1}", color.ToString(), playerUID);
        GhostPlayer targetPlayer = PlayerManager.Instance().GetByUID(playerUID);
        targetPlayer.MeepleColor = color;
        targetPlayer.colorIsClaimed = true;
        NotifyObservers();

        if (PlayerManager.Instance().local.isLobbyHost)
        {
            available.Remove(color);
        }
    }

    public void UnClaimColor(MeepleColor color)
    {
        Console.WriteLine("unclaiming {0}", color.ToString());

        if (PlayerManager.Instance().local.isLobbyHost)
        {
            available.Add(color);
        }
    }

    /// <summary>
    /// Color Request.
    /// Issued by Non-Lobby-Host, handled by Lobby-Host
    /// </summary>
    public void TryClaimNext(string playerUID)
    {
        if (PlayerManager.Instance().local.isLobbyHost)
        {
            TryClaim(playerUID, Next());
        }
    }
    
    /// <summary>
    /// Handling color request.
    /// Only for lobby host.
    /// </summary>
    public void TryClaim(string playerUID, MeepleColor color)
    {
        if (PlayerManager.Instance().local.isLobbyHost)
        {
            Console.WriteLine("i am lobby host, i have color power");

            if (available.Contains(color))
            {
                // send word to all other players and claim color
                ColorClaimedCommand cmd = new ColorClaimedCommand(playerUID, (int)color);
                PlayerManager.Instance().local.HandleInput(cmd, true);
            }
            else if (available.Count > 0)
            {
                Console.WriteLine(color.ToString() + " not available, claiming next");
                TryClaimNext(playerUID);
            }
            else
            {
                Console.WriteLine("out of colors");
            }
        }
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
        foreach(IObserver observer in observers)
        {
            observer.Update();
        }
    }


    private static MeepleColorClaimer instance;
    public static MeepleColorClaimer Instance()
    {
        if (instance == null)
        {
            instance = new MeepleColorClaimer();
        }
        return instance;
    }
}