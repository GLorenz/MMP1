using System;
using Microsoft.Xna.Framework;

public class InputHandler : IInputObserver
{
    private Player player;
    private Client playerClient;

    public InputHandler(Player player, Client playerClient)
    {
        this.player = player;
        this.playerClient = playerClient;
    }

    public void HandleInput(Input input)
    {
        HandleLocalInput(input);

        if(input.shouldShare)
        {
            playerClient.Share(input);
        }
    }

    private void HandleLocalInput(Input input)
    {
        switch (input.typeName)
        {
            case "Move":
                try
                {
                    MovingBoardElement movingEl = (MovingBoardElement)Board.Instance().FindByIdentifier(input.targetIdentifier);
                    string[] positions = input.body.Split(',');
                    Vector2 moveTo = new Vector2(float.Parse(positions[0]), float.Parse(positions[1]));

                    MoveCommand moveCmd = new MoveCommand(movingEl, moveTo);
                    CommandQueue.Instance().QueueCommand(moveCmd);
                }
                catch (InvalidCastException ice)
                {
                    Console.WriteLine(ice.Message);
                }
                break;
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