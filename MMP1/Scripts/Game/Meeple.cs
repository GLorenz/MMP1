using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Meeple : MovingBoardElement
{
    private Player player;

    public Meeple(Player player, string UID, Rectangle position, Texture2D texture) : base(UID, position, texture)
    {
        this.player = player;
    }

    public override void OnClick()
    {
        //idk, show player stats or smth
    }
}