public interface IToSerializableCommand : ICommand
{
    SerializableCommand ToSerializable(bool shouldShare);
}