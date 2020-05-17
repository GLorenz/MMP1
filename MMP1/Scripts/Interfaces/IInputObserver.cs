// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public interface IInputObserver : IObserver
{
    void update(SerializableCommand input);
}
