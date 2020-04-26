using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Client : IInputObservable
{
    public static readonly int bufferSize = 2048;

    public Socket socket { get; private set; }
    public List<IObserver> observers { get; set; }
    // private ManualResetEvent resetEvent;

    public Client()
    {
        observers = new List<IObserver>();
        // resetEvent = new ManualResetEvent(false);
    }

    public bool Connect()
    {
        Socket serverSock = ServerConnector.ConnectToServerSocket();

        if (serverSock.Connected)
        {
            this.socket = serverSock;

            // synchonosly in own thread:
            new Thread(() => ListenToSocket()).Start();

            // asynchonosly:
            // ReadStateObject state = new ReadStateObject();
            // state.socket = socket;
            // socket.BeginReceive(state.buffer, 0, bufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }
        return serverSock.Connected;
    }

    public void Share(Input input)
    {
        socket.Send(Serializer.SerializeInput(input));
    }

    public void OnReceive(byte[] data)
    {
        Input input = Serializer.Deserialize(data);
        input.shouldShare = false;
        notifyObservers(input);
    }

    // runs in own thread
    private void ListenToSocket()
    {
        Console.WriteLine("client listening to socket");
        byte[] buffer = new byte[bufferSize];
        while(socket.Connected)
        {
            socket.Receive(buffer);
            OnReceive(buffer);
        }
    }

    // asynchornos callback
    private void ReceiveCallback(IAsyncResult ar)
    {
        ReadStateObject state = (ReadStateObject)ar.AsyncState;
        int read = state.socket.EndReceive(ar);
        OnReceive(state.buffer);
        socket.BeginReceive(state.buffer, 0, bufferSize, 0, new AsyncCallback(ReceiveCallback), state);
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

    public class ReadStateObject
    {
        public Socket socket = null;
        public byte[] buffer = new byte[bufferSize];
    }
}