﻿// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Lobby
{
    private int id;

    private object historyLock = new object();
    private object socketLock = new object();

    private List<byte[]> history;
    private List<Socket> sockets;
    private int bufferSize;
    private string lobbyHostString;
    private int sendTickRateMS;
    private bool shouldRun;

    public static readonly string upToDateString = "uptodate";

    public Lobby(int id, string lobbyHostString, int bufferSize, int sendTickRateMS)
    {
        this.id = id;
        this.bufferSize = bufferSize;
        this.lobbyHostString = lobbyHostString;
        this.sendTickRateMS = sendTickRateMS;
        shouldRun = true;

        sockets = new List<Socket>();
        history = new List<byte[]>();
    }

    public void AddSocket(Socket socket)
    {
        lock (socketLock)
        {
            sockets.Add(socket);
            socket.SendBufferSize = bufferSize;
            socket.ReceiveBufferSize = bufferSize;
            new Task(() => ListenToSocket(socket)).Start();
        }

        // new player should get all previously sent data
        Print(string.Format("sending {0} packets of history", history.Count));

        SendHistory(socket);

        if (sockets.Count == 1)
        {
            Print("sending lobby host package");
            socket.Send(Encoding.ASCII.GetBytes(lobbyHostString));
        }
    }

    private async void SendHistory(Socket socket)
    {
        List<byte[]> curCommands;

        // copy list so command list isnt locked for the whole process
        lock (historyLock)
        {
            curCommands = new List<byte[]>(history);
        }

        foreach (byte[] command in curCommands)
        {
            socket.Send(command);
            await Task.Delay(sendTickRateMS);
        }

        socket.Send(Encoding.ASCII.GetBytes(upToDateString));
    }

    private void ListenToSocket(Socket socket)
    {
        while (socket != null && socket.Connected && shouldRun)
        {
            try
            {
                byte[] buffer = new byte[bufferSize];
                int read = socket.Receive(buffer);
                if (read > 0)
                {
                    new Task(() => OnSocketReceive(buffer, socket)).Start();
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
                    if (s != socket && s.Connected)
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
        if (socket != null)
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

    public void ShutdownAll()
    {
        shouldRun = false;
        lock (socketLock)
        {
            sockets.ForEach(s => { s.Shutdown(SocketShutdown.Both); s.Close(); });
        }
    }

    public int GetSocketCount()
    {
        return sockets.Count;
    }

    private void Print(string s)
    {
        Console.WriteLine(s);
        Console.Write("Lobby#"+id+" >");
    }
}