// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public class RemoveGhostPlayerCommand : INetworkCommand
{
    public const string name = "REM_GPLAYER";
    private string playerUID;

    public RemoveGhostPlayerCommand(string uid)
    {
        playerUID = uid;
    }

    public void Execute()
    {
        PlayerManager.Instance().GetByUID(playerUID).Destroy();
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand(name, playerUID, string.Empty, shouldShare);
    }

    public static RemoveGhostPlayerCommand FromSerializable(SerializableCommand sCommand)
    {
        return new RemoveGhostPlayerCommand(sCommand.UID);
    }
}