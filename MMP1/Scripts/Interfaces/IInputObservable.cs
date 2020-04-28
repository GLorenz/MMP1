public interface IInputObservable : IObservable
{
    void notifyObservers(SerializableCommand input);
}