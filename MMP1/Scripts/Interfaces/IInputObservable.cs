// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public interface IInputObservable : IObservable
{
    void NotifyObservers(SerializableCommand input);
}