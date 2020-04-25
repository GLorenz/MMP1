using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

public class Client : IInputObservable
{
    public Socket socket { get; private set; }
    public List<IObserver> observers { get; set; }

    public Client()
    {
        observers = new List<IObserver>();
    }

    public bool Connect()
    {
        Socket serverSock = ServerConnector.ConnectToServerSocket();

        if (serverSock.Connected)
        {
            this.socket = serverSock;
        }
        return serverSock.Connected;
    }

    public void Share(Input input)
    {
         socket.Send(Serializer.SerializeInput(input));
    }

    public void Receive(byte[] data)
    {
        notifyObservers(Serializer.Deserialize(data));
    }

    // observer methods
    public void AddObserver(IObserver observer)
    {
        if(!observers.Contains(observer)) { observers.Add(observer); }
    }

    public void RemoveObserver(IObserver observer)
    {
        if (observers.Contains(observer)) { observers.Remove(observer); }
    }

    public void notifyObservers()
    {
        observers.ForEach(o => o.update());
    }

    public void notifyObservers(Input input)
    {
        observers.ForEach(o => {
            if(o is IInputObserver)
            {
                ((IInputObserver)o).update(input);
            }
            else
            {
                o.update();
            }
        });
    }
}