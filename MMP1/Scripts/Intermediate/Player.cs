using Microsoft.Xna.Framework;
using System;

public class Player : GhostPlayer, IInputObserver
{
    public Client client { get; private set; }
    public bool isLobbyHost { get; private set; }

    public Player(string name, string UID = "") : this(name, new Client(), UID) { }

    public Player(string name, Client client, string UID = "") : base (name, UID)
    {
        this.client = client;
    }

    public void ConnectClient()
    {
        bool connected = false;
        if (Game1.networkType == Game1.NetworkType.Online)
        {
            connected = client.Connect();
            client.AddObserver(this);
        }
        Console.WriteLine("Connecting player {0}, Status: {1}", name, connected);
    }

    public override void Create()
    {
        OnlyShare(new CreateGhostPlayerCommand(name, UID));
        HandleInput(new ColorRequestedCommand(UID), true);
        base.Create();
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
        Console.WriteLine("Handling " + sCommand.typeName);
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

    protected override void CreateMeeples()
    {
        for (int i = 0; i < 2; i++)
        {
            Meeple newMeep = new Meeple(this, new Rectangle(0, 0, Board.Instance().boardUnit, Board.Instance().boardUnit), UID + "_meeple" + i, meepidx:i);
            newMeep.Create();
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
        input.shouldShare = false;
        HandleInput(input);
    }

    public void Update()
    {
        isLobbyHost = true;
        Console.WriteLine(UID + " is now lobby host");
    }
}