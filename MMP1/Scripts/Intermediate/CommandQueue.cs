using System;

public class CommandQueue
{
    private static object gameLock = new object();

    public static void Queue(ICommand command)
    {
        lock (gameLock)
        {
            command.execute();
        }
    }
}