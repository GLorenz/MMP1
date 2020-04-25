public interface IInputObservable : IObservable
{
    void notifyObservers(Input input);
}