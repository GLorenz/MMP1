// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using System;
using System.Collections.Generic;
using System.Threading;

public class MeepleColorClaimer
{
    private static List<MeepleColor> available = new List<MeepleColor>() { MeepleColor.Black, MeepleColor.Green, MeepleColor.Red, MeepleColor.White };

    public static MeepleColor Next()
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

    public static void ClaimColor(string playerUID, MeepleColor color)
    {
        Console.WriteLine("claiming {0} for {1}", color.ToString(), playerUID);
        PlayerManager.Instance().GetByUID(playerUID).MeepleColor = color;
        available.Remove(color);
    }

    public static void TryClaimNext(string playerUID)
    {
        TryClaim(playerUID, Next());
    }
    
    public static void TryClaim(string playerUID, MeepleColor color)
    {
        if (!PlayerManager.Instance().local.isLobbyHost) { return; }

        Console.WriteLine("i am lobby host, i have color power");

        if(available.Contains(color))
        {
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