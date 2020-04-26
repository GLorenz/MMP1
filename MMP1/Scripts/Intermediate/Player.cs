using System;

public class Player : IInputObserver
{
    public static Player local;

    public string name { get; private set; }
    public Client client { get; private set; }

    public Player(string name) : this(name, new Client()) {
        
    }

    public Player(string name, Client client)
    {
        this.name = name;
        this.client = client;

        bool connected = client.Connect();
        Console.WriteLine("Connecting player {0}, Status: {1}", name, connected);
        local = this;
    }

    public void HandleInput(Input input)
    {
        InputHandler.HandleLocalInput(input);
        if (input.shouldShare)
        {
            client.Share(input);
        }
    }

    public void HandleInput(IInputCommand command, bool shouldShare)
    {
        HandleInput(command.ToInput(shouldShare));
    }

    public void update(Input input)
    {
        HandleInput(input);
    }

    public void update()
    {
        //dont know what to do :(
    }
}