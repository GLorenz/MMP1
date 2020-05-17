// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public interface INetworkCommand : ICommand
{
    SerializableCommand ToSerializable(bool shouldShare);
}