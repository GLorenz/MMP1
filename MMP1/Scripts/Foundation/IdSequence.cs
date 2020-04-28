using System.Threading;

public class IdSequence
{
    private static int curInputId = -1;
    private static int curPlayerId = -1;

    public static int NextInputId()
    {
        return Interlocked.Increment(ref curInputId);
    }

    public static int NextPlayerId()
    {
        return Interlocked.Increment(ref curPlayerId);
    }
}