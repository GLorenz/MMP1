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

        Console.Write("> ");
        string input = Console.ReadLine();
        if(input.ToLower().Trim().Equals("stop")) { server.Stop(); }
        else { Console.WriteLine("unrecognised"); }
    }

    public static readonly IPAddress listenerIp = IPAddress.Any;
    public static readonly int port = 40400;

    public static readonly int bufferSize = 2048;
    public static readonly string lobbyHostString = "lobbyhost";

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
        new Task(() => ListenForNewSockets()).Start();
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
            //readStateObj.Reset();
            try
            {
                Console.WriteLine("Listening...");
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
                //socket.BeginReceive(readStateObj.buffer, 0, bufferSize, 0, new AsyncCallback(ReceiveCallback), readStateObj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                RemoveSocket(socket);
            }
            /*finally
            {
                // hold task until event is reset (happens when some message is received)
                readStateObj.mre.WaitOne();
            }*/
        }
    }

    // asynchornos callback
    private void ReceiveCallback(IAsyncResult ar)
    {
        ReadStateObject state = (ReadStateObject)ar.AsyncState;
        try
        {
            int read = state.socket.EndReceive(ar);
            if (read > 0)
            {
                //OnSocketReceive(state.);
                state.mre.Set();
            }
            else
            {
                RemoveSocket(state.socket);
            }
        }
        catch (SocketException se)
        {
            Console.WriteLine(se.Message);
            RemoveSocket(state.socket);
        }
    }

    private void OnSocketReceive(byte[] data, Socket socket)
    {
        history.Add(data);

        // distribute data to all other sockets
        for (int i = sockets.Count - 1; i >= 0; i--)
        {
            try
            {
                Socket s = sockets[i];
                if (s.Connected && s != socket)
                {
                    SendStateObject sendState = new SendStateObject();
                    sendState.socket = s;
                    //s.BeginSend(readState.buffer, 0, readState.buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), sendState);
                    s.Send(data);
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
        socket.SendBufferSize = bufferSize;
        socket.ReceiveBufferSize = bufferSize;
        new Task(() => ListenToSocket(socket)).Start();
        
        // new player should get all previously sent data
        Console.WriteLine("sending {0} packets of history", history.Count);

        

        SendStateObject sendState = new SendStateObject();
        sendState.socket = socket;
        foreach(byte[] command in history)
        {
            //sendState.data = command;
            socket.Send(command);
            //socket.BeginSend(sendState.data, 0, sendState.data.Length, SocketFlags.None, new AsyncCallback(SendCallback), sendState);
        }
        if (sockets.Count == 1)
        {
            Console.WriteLine("sending lobby host package");
            socket.Send(Encoding.ASCII.GetBytes(lobbyHostString));
        }
    }

    private void SendCallback(IAsyncResult ar)
    {
        SendStateObject sendState = (SendStateObject)ar.AsyncState;
        sendState.socket.EndSend(ar);
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
    public class SendStateObject
    {
        public Socket socket;
        public byte[] data;
    }
}