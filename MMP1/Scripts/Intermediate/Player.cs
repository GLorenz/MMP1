using System;

public class Player : IInputObserver
{
    public string name { get; private set; }
    private Client client;

    public Player(string name) : this(name, new Client()) {
        
    }

    public Player(string name, Client client)
    {
        this.name = name;
        this.client = client;

        bool connected = client.Connect();
        Console.WriteLine("Connecting player {0}, Status: {1}", name, connected);
    }

    public void HandleInput(Input input)
    {
        InputHandler.HandleLocalInput(input);
        if (input.shouldShare)
        {
            client.Share(input);
        }
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