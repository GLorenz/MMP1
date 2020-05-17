public class LulzCommand : INetworkCommand
{
    public void Execute()
    {
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand("lul", "lul", "lull", shouldShare);
    }
}