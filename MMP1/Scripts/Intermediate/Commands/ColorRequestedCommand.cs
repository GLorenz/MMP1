using System;

public class ColorRequestedCommand : INetworkCommand
{
    public const string name = "CLR_REQ";

    protected string playerUID;

    public ColorRequestedCommand(string playerUID)
    {
        this.playerUID = playerUID;
    }

    public virtual void Execute()
    {
        Console.WriteLine("command requesting next for {0} ", playerUID);
        MeepleColorClaimer.TryClaimNext(playerUID);
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand(name, playerUID, string.Empty, shouldShare);
    }

    public static ColorRequestedCommand FromSerializable(SerializableCommand sCommand)
    {
        return new ColorRequestedCommand(sCommand.UID);
    }
}