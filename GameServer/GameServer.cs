using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Threading.Tasks;

public class GameServer
{
    static void Main(string[] args)
    {
        GameServer server = new GameServer();
        server.Start();

        Console.Write("> ");
        string input = Console.ReadLine();
        if(input.ToLower().Trim().Equals("stop")) { server.Stop(); }
        else { Console.WriteLine("unrecognised"); }
    }

    public static readonly IPAddress listenerIp = IPAddress.Any;
    public static readonly int port = 40400;
    public static readonly string hostName = "5hos.ddns.net";

    public static readonly int bufferSize = 1024;

    public static readonly byte[] helloRequestPacket = new byte[] { 1, 2, 3, 4 };
    public static readonly byte[] helloResponsePacket = new byte[] { 4, 3, 2, 1 };

    private List<Socket> sockets;
    private bool shouldRun;
    private bool acceptNewSockets;

    private List<byte[]> history;

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
        new Thread(ListenForNewSockets).Start();
    }

    public void Stop()
    {
        shouldRun = false;
        sockets.ForEach(s => { s.Shutdown(SocketShutdown.Both); s.Close(); });
    }

    private void ListenToSocket(Socket socket)
    {
        ReadStateObject readStateObj = new ReadStateObject(socket);

        while (socket != null && socket.Connected && shouldRun)
        {
            readStateObj.Reset();
            try
            {
                Console.WriteLine("Listening...");
                socket.BeginReceive(readStateObj.buffer, 0, bufferSize, 0, new AsyncCallback(ReceiveCallback), readStateObj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // hold task until event is reset (happens when some message is received)
                readStateObj.mre.WaitOne();
            }
        }
    }

    // asynchornos callback
    private void ReceiveCallback(IAsyncResult ar)
    {
        ReadStateObject state = (ReadStateObject)ar.AsyncState;
        int read = state.socket.EndReceive(ar);
        if(read > 0)
        {
            OnSocketReceive(state);
        }
        else
        {
            RemoveSocket(state.socket);
        }
        state.mre.Set();
    }

    private void OnSocketReceive(ReadStateObject readState)
    {
        history.Add(readState.buffer);

        // distribute data to all other sockets
        for (int i = sockets.Count - 1; i >= 0; i--)
        {
            try
            {
                Socket s = sockets[i];
                if (s.Connected && s != readState.socket)
                {
                    s.Send(readState.buffer);
                    Console.WriteLine("sending bytes to another socket");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    private void AddSocket(Socket socket)
    {
        sockets.Add(socket);
        // new player should get all previously sent data
        foreach(byte[] command in history)
        {
            socket.Send(command);
        }
        new Task(() => ListenToSocket(socket)).Start();
    }

    private void RemoveSocket(Socket socket)
    {
        if(socket != null)
        {
            sockets.Remove(socket);

            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Console.WriteLine("Socket Disconnected");

            if (sockets.Count <= 0)
            {
                Console.WriteLine("clearing history");
                history.Clear();
            }
        }
    }

    private void ListenForNewSockets()
    {
        TcpListener server = new TcpListener(listenerIp, port);
        server.AllowNatTraversal(true);
        server.Start();
        Console.WriteLine("Started Game Server!");

        while (shouldRun && acceptNewSockets)
        {
            //blocking until there is a client
            try
            {
                Socket sock = server.AcceptSocket();
                Console.WriteLine("Accepted new Socket!");
                // if new is accepted, add to list and listen to it
                AddSocket(sock);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        server.Stop();
        Console.WriteLine("Stopped Server");
    }

    private static bool IsHelloRequestPackage(byte[] bytes)
    {
        if (bytes == null || bytes.Length != helloRequestPacket.Length) { return false; }
        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i] != helloRequestPacket[i])
            {
                return false;
            }
        }
        return true;
    }

    public class ReadStateObject
    {
        public Socket socket;
        public ManualResetEvent mre;
        public byte[] buffer;

        public ReadStateObject(Socket socket)
        {
            this.socket = socket;
            mre = new ManualResetEvent(false);
        }

        public void Reset()
        {
            //mre = new ManualResetEvent(false);
            mre.Reset();
            buffer = new byte[bufferSize];
        }
    }
}