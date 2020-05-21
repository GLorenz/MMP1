// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using System;

public class UnClaimColorCommand : INetworkCommand
{
    public const string name = "CLR_UNCLAIM";
    
    protected int color;

    public UnClaimColorCommand(int color)
    {
        this.color = color;
    }

    public virtual void Execute()
    {
        MeepleColorClaimer.Instance().UnClaimColor((MeepleColor)color);
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand(name, string.Empty, color.ToString(), shouldShare);
    }

    public static UnClaimColorCommand FromSerializable(SerializableCommand sCommand)
    {
        return new UnClaimColorCommand(int.Parse(sCommand.body));
    }
}