using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Client : IInputObservable
{
    public static readonly int bufferSize = 1024;

    public Socket socket { get; private set; }
    public List<IObserver> observers { get; set; }
    ReadStateObject readStateObj;

    public Client()
    {
        observers = new List<IObserver>();
        readStateObj = new ReadStateObject();
    }

    public bool Connect()
    {
        Socket serverSock = ServerConnector.ConnectToServerSocket();

        if (serverSock.Connected)
        {
            this.socket = serverSock;

            // asynchonosly:
            ListenForNext();
        }
        return serverSock.Connected;
    }

    public void Disconnect()
    {
        if(socket != null)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }

    public void Share(SerializableCommand input)
    {
        socket.Send(Serializer.SerializeInput(input));
    }

    public void OnReceive(byte[] data)
    {
        SerializableCommand input = Serializer.Deserialize(data);
        if (input != null)
        {
            input.shouldShare = false;
            notifyObservers(input);
        }
    }

    public void ListenForNext()
    {
        if (socket != null && socket.Connected)
        {
            readStateObj.Reset();
            readStateObj.socket = socket;
            socket.BeginReceive(readStateObj.buffer, 0, bufferSize, 0, new AsyncCallback(ReceiveCallback), readStateObj);
        }
    }

    // asynchornos callback
    private void ReceiveCallback(IAsyncResult ar)
    {
        ReadStateObject state = (ReadStateObject)ar.AsyncState;
        if (socket != null && socket.Connected)
        {
            int read = state.socket.EndReceive(ar);
            OnReceive(state.buffer);
            ListenForNext();
        }
    }

// runs in own thread
    /*private void ListenToSocket()
    {
        Console.WriteLine("client listening to socket");
        byte[] buffer = new byte[bufferSize];
        while(socket.Connected)
        {
            socket.Receive(buffer);
            OnReceive(buffer);
        }
    }*/
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

    public void notifyObservers(SerializableCommand input)
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

        public void Reset()
        {
            socket = null;
            buffer = new byte[bufferSize];
        }
    }
}