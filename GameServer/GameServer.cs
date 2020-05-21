// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

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

    public static readonly int bufferSize = 512;
    public static readonly string lobbyHostString = "lobbyhost";

    public static readonly int sendTickRateMS = 1000 / 100;

    private List<Lobby> lobbies;
    private Lobby nextLobby;

    public bool shouldRun { get; private set; }
    private bool acceptNewSockets;

    private object socketLock = new object();

    public GameServer()
    {
        shouldRun = false;
        acceptNewSockets = true;
        lobbies = new List<Lobby>();
    }

    public void Start()
    {
        shouldRun = true;
        new Task(() => ListenForNewSockets()).Start();
    }

    public void Stop()
    {
        shouldRun = false;
        foreach(Lobby lobby in lobbies)
        {
            lobby.ShutdownAll();
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
                if(nextLobby == null || nextLobby.GetSocketCount() >= 4)
                {
                    Lobby newLobby = new Lobby(lobbies.Count, lobbyHostString, bufferSize, sendTickRateMS);
                    lobbies.Add(newLobby);
                    nextLobby = newLobby;
                    Console.WriteLine("Created new Lobby");
                }
                //new Task(()=>nextLobby.AddSocket(sock)).Start();
                nextLobby.AddSocket(sock);
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

    private static void Print(string s)
    {
        Console.WriteLine(s);
        Console.Write("Server > ");
    }
}