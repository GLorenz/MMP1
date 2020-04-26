using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System;

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

    public GameServer()
    {
        sockets = new List<Socket>();
        shouldRun = false;
        acceptNewSockets = true;
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
        while (socket.Connected && shouldRun)
        {
            byte[] buffer = new byte[bufferSize];
            try
            {
                socket.Receive(buffer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // distribute data to all other sockets
            sockets.ForEach(s =>
            {
                if (s != socket)
                {
                    s.Send(buffer);
                }
            });

            /*if (IsHelloRequestPackage(buffer))
            {
                // answer the socket with hello response and terminate socket
                socket.Send(helloResponsePacket);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            else
            {
                // distribute data to all other sockets
                sockets.ForEach(s =>
                {
                    if (s != socket)
                    {
                        s.Send(buffer);
                    }
                });
            }*/
        }
    }

    private void AddSocket(Socket socket)
    {
        sockets.Add(socket);
        new Thread(() => ListenToSocket(socket)).Start();
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
}