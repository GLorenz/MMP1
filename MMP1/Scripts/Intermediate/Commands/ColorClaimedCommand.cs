// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using System;

class ColorClaimedCommand : INetworkCommand
{
    public const string name = "CLR_CLAIM";

    protected string playerUID;
    protected int color;

    public ColorClaimedCommand(string playerUID, int color)
    {
        this.playerUID = playerUID;
        this.color = color;
    }

    public virtual void Execute()
    {
        MeepleColorClaimer.Instance().ClaimColor(playerUID, (MeepleColor)color);
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