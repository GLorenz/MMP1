using System.Net;
using System.Net.Sockets;

public class ServerConnector
{
    public static readonly int port = 40400;
    public static readonly string hostName = "5hos.ddns.net";

    // method from: https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket?view=netcore-3.1
    public static Socket ConnectToServerSocket()
    {

        if(Game1.networkType == Game1.NetworkType.Online)
        {
            return Connect(hostName);
        }
        else if(Game1.networkType == Game1.NetworkType.Local)
        {
            IPEndPoint ipe = new IPEndPoint(IPAddress.Loopback, port);
            if (TryConnectTo(ipe, out Socket socket))
            {
                return socket;
            }
        }
        return null;
    }

    private static Socket Connect(string hostname)
    {
        Socket s = null;
        IPHostEntry hostEntry = null;

        // Get host related information.
        hostEntry = Dns.GetHostEntry(hostName);

        // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
        // an exception that occurs when the host IP Address is not compatible with the address family
        // (typical in the IPv6 case).
        foreach (IPAddress address in hostEntry.AddressList)
        {
            if (TryConnectTo(new IPEndPoint(address, port), out Socket tempSocket))
            {
                s = tempSocket;
                break;
            }
            else
            {
                continue;
            }
        }
        return s;
    }

    private static bool TryConnectTo(IPEndPoint ipe, out Socket socket) {
        socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        socket.Connect(ipe);

        return socket.Connected;
    }
}