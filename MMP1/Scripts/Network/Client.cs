using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Client : IInputObservable
{
    public static readonly int bufferSize = 2048;
    public static readonly int sendTickRateMS = 1000 / 20;
    public static readonly string lobbyHostString = "lobbyhost";

    public Socket socket { get; private set; }
    public List<IObserver> observers { get; set; }
    ReadStateObject readStateObj;
    SendStateObject sendStateObj;

    private Queue<SerializableCommand> sendQueue;
    private bool sendQueueIsWorking;

    private object gameLock = new object();

    public Client()
    {
        observers = new List<IObserver>();
        readStateObj = new ReadStateObject();
        sendStateObj = new SendStateObject();

        sendQueue = new Queue<SerializableCommand>();
        sendQueueIsWorking = false;
    }

    public bool Connect()
    {
        Socket serverSock = ServerConnector.ConnectToServerSocket();

        if (serverSock.Connected)
        {
            this.socket = serverSock;
            socket.ReceiveBufferSize = bufferSize;
            socket.SendBufferSize = bufferSize;

            sendStateObj.socket = socket;

            new Task(() => ListenForNext()).Start();
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
        sendQueue.Enqueue(input);
        new Task(() => WorkSendQueue()).Start();
    }

    private async void WorkSendQueue()
    {
        if (!sendQueueIsWorking)
        {
            sendQueueIsWorking = true;
            while (sendQueue.Count > 0)
            {
                socket.Send(Serializer.SerializeInput(sendQueue.Dequeue()));
                //socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), sendStateObj);
                await Task.Delay(sendTickRateMS);
            }
            sendQueueIsWorking = false;
        }
    }

    /*private void SendCallback(IAsyncResult ar)
    {
        SendStateObject sendState = (SendStateObject)ar.AsyncState;
        sendState.socket.EndSend(ar);
    }*/

    public void OnReceive(byte[] data)
    {
        if (Encoding.ASCII.GetString(data).StartsWith(lobbyHostString))
        {
            lock (gameLock)
            {
                Console.WriteLine("received lobby host package");
                notifyObservers();
            }
        }
        else
        {
            SerializableCommand input = Serializer.Deserialize(data);
            if (input != null)
            {
                lock (gameLock)
                {
                    notifyObservers(input);
                }
            }
        }
    }

    public void ListenForNext()
    {
        while (socket != null && socket.Connected)
        {
            /*readStateObj.Reset();
            readStateObj.socket = socket;
            socket.BeginReceive(readStateObj.buffer, 0, bufferSize, 0, new AsyncCallback(ReceiveCallback), readStateObj);*/

            byte[] data = new byte[bufferSize];
            try
            {
                Console.WriteLine("listening for next");
                int read = socket.Receive(data);
                if (read > 0)
                {
                    OnReceive(data);
                }
            }
            catch(SocketException) { }
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
    public class SendStateObject
    {
        public Socket socket;
    }
}