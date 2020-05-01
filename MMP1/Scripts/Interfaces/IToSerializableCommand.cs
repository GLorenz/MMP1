public interface INetworkCommand : ICommand
{
    SerializableCommand ToSerializable(bool shouldShare);
}