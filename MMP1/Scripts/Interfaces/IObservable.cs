using System.Collections.Generic;

public interface IObservable
{
    List<IObserver> observers { get; set; }
    void AddObserver(IObserver observer);
    void RemoveObserver(IObserver observer);
    void NotifyObservers();
}
