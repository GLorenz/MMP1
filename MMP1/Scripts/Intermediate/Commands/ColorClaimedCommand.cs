using System;

class ColorClaimedCommand : INetworkCommand
{
    public const string name = "CLRCLAIM";

    protected string playerUID;
    protected int color;

    public ColorClaimedCommand(string playerUID, int color)
    {
        this.playerUID = playerUID;
        this.color = color;
    }

    public virtual void Execute()
    {
        Console.WriteLine("command execute trying to claim {0} for {1}", ((MeepleColor)color).ToString(), playerUID);
        MeepleColorClaimer.ClaimColor(playerUID, (MeepleColor)color);
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand(name, playerUID, color.ToString(), shouldShare);
    }

    public static ColorClaimedCommand FromSerializable(SerializableCommand sCommand)
    {
        return new ColorClaimedCommand(sCommand.UID, int.Parse(sCommand.body));
    }
}