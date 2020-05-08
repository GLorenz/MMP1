using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Meeple : GhostMeeple
{
    public Player player { get; protected set; }

    public Meeple(Player player, Rectangle position, MeepleColor color, int zPosition = 0, int UID = 0) : base(player, position, color, zPosition, UID) { }

    public Meeple(Player player, Rectangle position, Texture2D texture, int zPosition = 0, int UID = 0) : base(player, position, texture, zPosition, UID)
    {
        this.player = player;
    }

    public override void OnClick()
    {
        QuickTimeMovement.Instance().Toggle(this);
    }
}