// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public interface IInputObservable : IObservable
{
    void notifyObservers(SerializableCommand input);
}