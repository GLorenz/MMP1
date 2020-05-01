using System.Threading;

public class MeepleColorClaimer
{
    private static int claimedUntil = -1;

    public static MeepleColor Next()
    {
        if(claimedUntil < Game1.playerCount-1)
        {
            return (MeepleColor)Interlocked.Increment(ref claimedUntil);
        }
        else
        {
            return 0;
        }
    }
}