// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Client : IInputObservable
{
    public static readonly int bufferSize = 512;
    public static readonly int sendTickRateMS = 1000 / 50;

    public static readonly string lobbyHostString = "lobbyhost";
    public static readonly string upToDateString = "uptodate";
    public static readonly string disconnectString = "disconnect";

    public Socket socket { get; private set; }
    public List<IObserver> observers { get; set; }

    private object socketLock = new object();

    private Queue<byte[]> sendQueue;
    private bool sendQueueIsWorking;
    private bool disconnectMe;

    public bool upToDateReceived { get; private set; }
    public bool lobbyHostReceived { get; private set; }

    public Client()
    {
        upToDateReceived = false;
        observers = new List<IObserver>();
        
        sendQueue = new Queue<byte[]>();
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

            new Task(() => ListenForNext()).Start();
        }
        return serverSock.Connected;
    }

    public void Disconnect()
    {
        if (socket != null)
        {
            // activley blocking thread until sending is done
            while(sendQueueIsWorking)
            {
                Thread.Sleep(sendTickRateMS);
            }
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }

    public void ListenForNext()
    {
        while (socket != null && socket.Connected)
        {
            byte[] data = new byte[bufferSize];
            try
            {
                int read = socket.Receive(data);
                if (read > 0)
                {
                    // new Task so i can listen for next package
                    new Task(()=>OnReceive(data)).Start();
                }
            }
            catch(SocketException) { }
        }
    }

    public void Share(SerializableCommand input)
    {
        Share(Serializer.SerializeInput(input));
    }

    private void Share(byte[] message)
    {
        sendQueue.Enqueue(message);
        StartSendQueue();
    }

    private void StartSendQueue()
    {
        if(upToDateReceived)
        {
            new Task(() => WorkSendQueue()).Start();
        }
    }

    private async void WorkSendQueue()
    {
        if (!sendQueueIsWorking)
        {
            sendQueueIsWorking = true;
            while (sendQueue.Count > 0)
            {
                socket.Send(sendQueue.Dequeue());
                await Task.Delay(sendTickRateMS);
            }
            sendQueueIsWorking = false;
        }
    }

    public void OnReceive(byte[] data)
    {
        if (Encoding.ASCII.GetString(data).StartsWith(lobbyHostString))
        {
            lobbyHostReceived = true;
            Console.WriteLine("received lobby host package");
            NotifyObservers();
        }
        else if (Encoding.ASCII.GetString(data).StartsWith(upToDateString))
        {
            upToDateReceived = true;
            Console.WriteLine("im up to date");
            StartSendQueue();
            NotifyObservers();
        }
        else
        {
            SerializableCommand input = Serializer.Deserialize(data);
            if (input != null)
            {
                Console.WriteLine("received " + input.typeName);
                NotifyObservers(input);
            }
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

    public void NotifyObservers()
    {
        observers.ForEach(o => o.Update());
    }

    public void NotifyObservers(SerializableCommand input)
    {
        // not using foreach, since observers can remove themselves on execution
        // and modify observers list
        for (int i = observers.Count - 1; i >= 0; i--)
        {
            IObserver o = observers[i];
            if (o is IInputObserver)
            {
                ((IInputObserver)o).Update(input);
            }
            else
            {
                o.Update();
            }
        }
    }
}