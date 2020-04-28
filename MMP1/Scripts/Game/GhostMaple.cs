using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class GhostMeeple : MovingBoardElement
{
    public GhostPlayer ghostPlayer { get; protected set; }

    public GhostMeeple(GhostPlayer player, Rectangle position, Texture2D texture, int UID = 0) : base(position, texture, UID)
    {
        this.ghostPlayer = player;
    }

    public override void OnClick()
    {
        //idk, show player stats or smth
    }
}