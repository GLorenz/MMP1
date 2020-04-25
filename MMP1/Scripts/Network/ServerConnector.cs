using System.Net;
using System.Net.Sockets;

public class ServerConnector
{
    public static readonly int port = 40400;
    public static readonly string hostName = "5hos.ddns.net";

    // method from: https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.socket?view=netcore-3.1
    public static Socket ConnectToServerSocket()
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
            IPEndPoint ipe = new IPEndPoint(address, port);
            Socket tempSocket =
                new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            tempSocket.Connect(ipe);

            if (tempSocket.Connected)
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
}