// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

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