public interface IInputObserver : IObserver
{
    void update(SerializableCommand input);
}
