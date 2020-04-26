using System;
using Microsoft.Xna.Framework;

public class InputHandler
{
    public static void HandleLocalInput(Input input)
    {
        switch (input.typeName)
        {
            case "Move":
                try
                {
                    MovingBoardElement movingEl = (MovingBoardElement)Board.Instance().FindByUID(input.targetUID);
                    string[] positions = input.body.Split(',');
                    Point moveTo = new Point(int.Parse(positions[0]), int.Parse(positions[1]));

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
}