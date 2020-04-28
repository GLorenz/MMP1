using System.IO;
using System;

public class CreateGhostPlayerCommand : IToSerializableCommand
{
    public const string name = "CrGP";
    private GhostPlayer p;

    public CreateGhostPlayerCommand(GhostPlayer p)
    {
        this.p = p;
    }

    public void execute()
    {
        PlayerManager.Instance().AddGhost(p);
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand(name, p.UID, p.name, shouldShare);
    }

    public static CreateGhostPlayerCommand FromInput(SerializableCommand sCommand)
    {
        GhostPlayer ghost = new GhostPlayer(sCommand.body, sCommand.UID);
        return new CreateGhostPlayerCommand(ghost);
    }
}