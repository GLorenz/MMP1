// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using System.IO;
using System;

public class CreateGhostPlayerCommand : INetworkCommand
{
    public const string name = "CR_GPLAYER";
    private string playerName;
    private string playerUID;

    public CreateGhostPlayerCommand(string name, string uid)
    {
        playerName = name;
        playerUID = uid;
    }

    public void Execute()
    {
        new GhostPlayer(playerName, playerUID).Create();
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand(name, playerUID, playerName, shouldShare);
    }

    public static CreateGhostPlayerCommand FromSerializable(SerializableCommand sCommand)
    {
        return new CreateGhostPlayerCommand(sCommand.body, sCommand.UID);
    }
}