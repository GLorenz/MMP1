public class Player
{
    public string name { get; private set; }
    private InputHandler inputHandler;

    public Player(string name, Client client)
    {
        this.name = name;
        inputHandler = new InputHandler(this, client);
    }
}