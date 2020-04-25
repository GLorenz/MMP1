
public class CommandQueue
{
    private static CommandQueue cmdQueue;
    public static CommandQueue Instance()
    {
        if (cmdQueue == null) { cmdQueue = new CommandQueue(); }
        return cmdQueue;
    }
    private CommandQueue() { }

    public void QueueCommand(ICommand command)
    {
        //TODO: create Queue
        command.execute();
    }
}