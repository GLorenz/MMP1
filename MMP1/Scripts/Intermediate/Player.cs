using System;

public class Player : GhostPlayer, IInputObserver
{
    public Client client { get; private set; }

    public Player(string name, string UID) : this(name, new Client(), UID) { }

    public Player(string name, Client client, string UID) : base (name, UID)
    {
        this.client = client;
        ConnectClient();
    }

    private void ConnectClient()
    {
        bool connected = false;
        if (Game1.networkType == Game1.NetworkType.Online)
        {
            connected = client.Connect();
            client.AddObserver(this);
        }
        Console.WriteLine("Connecting player {0}, Status: {1}", name, connected);
    }

    public void DisconnectClient()
    {
        if(client != null)
        {
            client.RemoveObserver(this);
            client.Disconnect();
            Console.WriteLine("Disconnected player {0}", name);
        }
    }

    public void HandleInput(SerializableCommand sCommand)
    {
        Console.WriteLine(name + " handling input of type: " + sCommand.typeName);
        CommandHandler.Handle(sCommand);
        OnlyShare(sCommand);
    }

    public void OnlyShare(SerializableCommand sCommand)
    {
        if (Game1.networkType == Game1.NetworkType.Online && sCommand.shouldShare)
        {
            client.Share(sCommand);
        }
    }

    public void OnlyShare(INetworkCommand command)
    {
        OnlyShare(command.ToSerializable(true));
    }

    public void HandleInput(INetworkCommand command, bool shouldShare)
    {
        HandleInput(command.ToSerializable(shouldShare));
    }

    public void update(SerializableCommand input)
    {
        HandleInput(input);
    }

    public void update()
    {
        //dont know what to do :(
    }
}