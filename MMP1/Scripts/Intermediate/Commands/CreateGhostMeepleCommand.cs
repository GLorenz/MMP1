using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class CreateGhostMeepleCommand : INetworkCommand
{
    public const string name = "CrGMeep";
    GhostMeeple meeple;

    public CreateGhostMeepleCommand(GhostMeeple meeple)
    {
        this.meeple = meeple;
    }

    public void execute()
    {
        Board.Instance().AddElement(meeple);
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        Rectangle relRect = UnitConvert.ToScreenRelative(meeple.Position);
        string body = "";
        body += meeple.ghostPlayer.UID;
        body += ";" + relRect.X + "," + relRect.Y + "," + relRect.Width + "," + relRect.Height;
        // not needing to communicate texture since it just gets claimed
        // body += ";" + TextureResources.Get(meeple.texture);

        return new SerializableCommand(name, meeple.UID, body, shouldShare);
    }

    public static CreateGhostMeepleCommand FromSerializable(SerializableCommand sCommand)
    {
        try
        {
            BoardElement movingEl = Board.Instance().FindByUID(sCommand.UID);
            string[] boardElProps = sCommand.body.Split(';');

            GhostPlayer ghostPlayer = PlayerManager.Instance().GetByUID(int.Parse(boardElProps[0]));
            string[] rectPosis = boardElProps[1].Split(',');
            Rectangle rect = UnitConvert.ToAbsolute(new Rectangle(int.Parse(rectPosis[0]), int.Parse(rectPosis[1]), int.Parse(rectPosis[2]), int.Parse(rectPosis[3])));
            MeepleColor color = MeepleColorClaimer.Next();

            return new CreateGhostMeepleCommand(new GhostMeeple(ghostPlayer, rect, color, sCommand.UID));
        }
        catch (InvalidCastException ice)
        {
            Console.WriteLine(ice.Message);
            return null;
        }
    }
}