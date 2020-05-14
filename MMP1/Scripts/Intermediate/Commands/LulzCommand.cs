public class LulzCommand : INetworkCommand
{
    public void execute()
    {
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand("lul", "lul", "lull", shouldShare);
    }
}