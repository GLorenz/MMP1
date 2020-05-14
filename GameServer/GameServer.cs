using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Threading.Tasks;
using System.Text;

public class GameServer
{
    static void Main(string[] args)
    {
        GameServer server = new GameServer();
        server.Start();
        
        while (server.shouldRun)
        {
            string input = Console.ReadLine();

            if (input.ToLower().Trim().Equals("stop")) { server.Stop(); }
            else { Print("unrecognised"); }
        }
    }

    public static readonly IPAddress listenerIp = IPAddress.Any;
    public static readonly int port = 40400;

    public static readonly int bufferSize = 2048;
    public static readonly string lobbyHostString = "lobbyhost";

    private List<Socket> sockets;
    public bool shouldRun { get; private set; }
    private bool acceptNewSockets;

    private List<byte[]> history;

    private object socketLock = new object();
    private object historyLock = new object();

    public GameServer()
    {
        sockets = new List<Socket>();
        shouldRun = false;
        acceptNewSockets = true;

        history = new List<byte[]>();
    }

    public void Start()
    {
        shouldRun = true;
        new Task(() => ListenForNewSockets()).Start();
    }

    public void Stop()
    {
        shouldRun = false;
        lock (socketLock)
        {
            sockets.ForEach(s => { s.Shutdown(SocketShutdown.Both); s.Close(); });
        }
    }

    private void ListenForNewSockets()
    {
        TcpListener server = new TcpListener(listenerIp, port);
        server.AllowNatTraversal(true);
        server.Start();
        Print("Started Game Server!");

        while (shouldRun && acceptNewSockets)
        {
            //blocking until there is a client
            try
            {
                Socket sock = server.AcceptSocket();
                Print("Accepted new Socket!");
                // if new is accepted, add to list and listen to it
                AddSocket(sock);
            }
            catch (Exception e)
            {
                Print("exception at listen");
                Print(e.Message);
            }
        }

        server.Stop();
        Print("Stopped Server");
    }

    private void AddSocket(Socket socket)
    {
        lock (socketLock)
        {
            sockets.Add(socket);
        }

        socket.SendBufferSize = bufferSize;
        socket.ReceiveBufferSize = bufferSize;
        new Task(() => ListenToSocket(socket)).Start();
        
        // new player should get all previously sent data
        Print(string.Format("sending {0} packets of history", history.Count));

        lock (historyLock)
        {
            foreach (byte[] command in history)
            {
                socket.Send(command);
            }
        }
        if (sockets.Count == 1)
        {
            Print("sending lobby host package");
            socket.Send(Encoding.ASCII.GetBytes(lobbyHostString));
        }
    }

    private void ListenToSocket(Socket socket)
    {
        while (socket != null && socket.Connected && shouldRun)
        {
            try
            {
                Print("Listening...");
                byte[] buffer = new byte[bufferSize];
                int read = socket.Receive(buffer);
                if(read > 0)
                {
                    OnSocketReceive(buffer, socket);
                    ListenToSocket(socket);
                }
                else
                {
                    RemoveSocket(socket);
                }
            }
            catch (Exception e)
            {
                Print("exception at single socket listen");
                Print(e.Message);
                RemoveSocket(socket);
            }
        }
    }

    private void OnSocketReceive(byte[] data, Socket socket)
    {
        lock (historyLock)
        {
            history.Add(data);
        }

        lock (socketLock)
        {
            // distribute data to all other sockets
            for (int i = sockets.Count - 1; i >= 0; i--)
            {
                try
                {
                    Socket s = sockets[i];
                    if (s.Connected && s != socket)
                    {
                        s.Send(data);
                        Print("sending bytes to another socket");
                    }
                }
                catch (Exception e)
                {
                    Print("exception at onsocketreceive");
                    Print(e.Message);
                }
            }
        }
    }

    private void RemoveSocket(Socket socket)
    {
        if(socket != null)
        {
            lock (socketLock)
            {
                sockets.Remove(socket);
            }

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Print("Socket Disconnected");

            if (sockets.Count <= 0)
            {
                Print("clearing history");
                lock (historyLock)
                {
                    history.Clear();
                }
            }
        }
    }

    private static void Print(string s)
    {
        Console.WriteLine(s);
        Console.Write("> ");
    }
}