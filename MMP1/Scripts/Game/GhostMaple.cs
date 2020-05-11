using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class GhostMeeple : MovingBoardElement
{
    public GhostPlayer ghostPlayer { get; protected set; }

    public PyramidFloorBoardElement standingOn { get; protected set; }
    private PyramidFloorBoardElement startElement;

    private MeepleColor color;
    public MeepleColor Color
    {
        get
        {
            return color;
        }
        set
        {
            color = value;
            texture = Texture4Color(value);
        }
    }

    public GhostMeeple(GhostPlayer player, Rectangle position, MeepleColor color, int zPosition = 0, int UID = 0) : this(player, position, Texture4Color(color), zPosition, UID)
    {
        this.color = color;
    }

    public GhostMeeple(GhostPlayer player, Rectangle position, Texture2D texture, int zPosition = 0, int UID = 0) : base(position, texture, zPosition, UID)
    {
        this.color = Color4Texture(texture);
        this.ghostPlayer = player;
        PlayerManager.Instance().AddMeepleRef(this);
    }

    public override void MoveTo(PyramidFloorBoardElement element)
    {
        standingOn = element;
        MoveTo(element.Position.Location);
    }

    public void SetStartingElement(PyramidFloorBoardElement startElement)
    {
        this.startElement = startElement;
    }

    public void BackToStart()
    {
        MoveTo(startElement);
    }

    public override void OnClick()
    {
        //idk, show player stats or smth
    }

    private static Texture2D Texture4Color(MeepleColor color)
    {
        return TextureResources.Get("Player" + color.ToString());
    }

    private static MeepleColor Color4Texture(Texture2D texture)
    {
        Enum.TryParse(TextureResources.Get(texture).Replace("Player", ""), out MeepleColor color);
        return color;
    }
}