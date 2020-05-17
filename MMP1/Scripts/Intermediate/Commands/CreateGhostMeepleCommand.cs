using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class CreateGhostMeepleCommand : INetworkCommand
{   //todo: add zPositions to all commands
    public const string name = "CR_GMEEP";
    GhostMeeple meeple;

    public CreateGhostMeepleCommand(GhostMeeple meeple)
    {
        this.meeple = meeple;
    }

    public void Execute()
    {
        Console.WriteLine("adding ghost meeple 1");
        meeple.Create();
        Console.WriteLine("adding ghost meeple 2");
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        Rectangle relRect = UnitConvert.ToScreenRelative(meeple.Position);
        string body = "";
        body += meeple.ghostPlayer.UID;
        body += ";"+ meeple.standingOn.UID;
        body += ";" + meeple.meepIdx;
        //body += ";" + relRect.X + "," + relRect.Y + "," + relRect.Width + "," + relRect.Height;
        Console.WriteLine("creating command {0} {1} {2}", name, meeple.UID, body);
        return new SerializableCommand(name, meeple.UID, body, shouldShare);
    }

    public static CreateGhostMeepleCommand FromSerializable(SerializableCommand sCommand)
    {
        try
        {
            string[] boardElProps = sCommand.body.Split(';');

            GhostPlayer ghostPlayer = PlayerManager.Instance().GetByUID(boardElProps[0]);

            /*string[] rectPosis = boardElProps[1].Split(',');
            Rectangle rect = UnitConvert.ToAbsolute(new Rectangle(int.Parse(rectPosis[0]), int.Parse(rectPosis[1]), int.Parse(rectPosis[2]), int.Parse(rectPosis[3])));*/
            PyramidFloorBoardElement standingOn = (PyramidFloorBoardElement)Board.Instance().FindByUID(boardElProps[1]);
            int meepIdx = int.Parse(boardElProps[2]);
            Console.WriteLine("creating {0} standing on {1}", sCommand.UID, standingOn.UID);

            return new CreateGhostMeepleCommand(new GhostMeeple(ghostPlayer, standingOn, sCommand.UID, Meeple.defaultZPosition, meepIdx));
        }
        catch (InvalidCastException ice)
        {
            Console.WriteLine(ice.Message);
            return null;
        }
    }
}