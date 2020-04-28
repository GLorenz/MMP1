using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class CreateGhostMeepleCommand : IToSerializableCommand
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
        string body = "";
        body += meeple.ghostPlayer.UID;
        body += ";" + meeple.Position.X + "," + meeple.Position.Y + "," + meeple.Position.Width + "," + meeple.Position.Height;
        body += ";" + TextureResources.Get(meeple.texture);

        return new SerializableCommand(name, meeple.UID, body, shouldShare);
    }

    public static CreateGhostMeepleCommand FromInput(SerializableCommand sCommand)
    {
        try
        {
            BoardElement movingEl = Board.Instance().FindByUID(sCommand.UID);
            string[] boardElProps = sCommand.body.Split(';');

            GhostPlayer ghostPlayer = PlayerManager.Instance().GetByUID(int.Parse(boardElProps[0]));
            string[] rectPosis = boardElProps[1].Split(',');
            Rectangle rect = new Rectangle(int.Parse(rectPosis[0]), int.Parse(rectPosis[1]), int.Parse(rectPosis[2]), int.Parse(rectPosis[3]));
            Texture2D texture = TextureResources.Get(boardElProps[2]);

            return new CreateGhostMeepleCommand(new GhostMeeple(ghostPlayer, rect, texture, sCommand.UID));
        }
        catch (InvalidCastException ice)
        {
            Console.WriteLine(ice.Message);
            return null;
        }
    }
}