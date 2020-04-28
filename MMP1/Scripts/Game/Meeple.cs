using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Meeple : GhostMeeple
{
    public Player player { get; protected set; }

    public Meeple(Player player, Rectangle position, Texture2D texture, int UID = 0) : base(player, position, texture, UID)
    {
        this.player = player;
    }

    public override void OnClick()
    {
        //idk, show player stats or smth
    }
}