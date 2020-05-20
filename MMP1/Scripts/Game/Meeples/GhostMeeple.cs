// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class GhostMeeple : MovingBoardElement
{
    public const int defaultZPosition = 10;
    public GhostPlayer ghostPlayer { get; protected set; }

    public PyramidFloorBoardElement standingOn { get; protected set; }
    protected PyramidFloorBoardElement startElement;
    public bool hasWon { get; protected set; }

    private MeepleColor color;
    public int meepIdx { get; protected set; }

    public GhostMeeple(GhostPlayer player, PyramidFloorBoardElement standingOn, string UID, int zPosition = defaultZPosition, int meepIdx = 0) : this(player, standingOn.Position, UID, zPosition, meepIdx)
    {
        this.standingOn = standingOn;
    }

    public GhostMeeple(GhostPlayer player, Rectangle position, string UID, int zPosition = defaultZPosition, int meepIdx = 0) : base(position, Texture4Color(player.MeepleColor), UID, zPosition)
    {
        this.ghostPlayer = player;
        this.meepIdx = meepIdx;
        CommandQueue.Queue(new AddGhostMeepleToPlayerManager(this));
    }

    public virtual void Create()
    {
        CommandQueue.Queue(new AddToBoardCommand(this));
        Color = ghostPlayer.MeepleColor;
        Console.WriteLine("created ghost meeple "+UID);
    }

    public override void MoveToLocalOnly(PyramidFloorBoardElement element)
    {
        standingOn = element;
        base.MoveToLocalOnly(element);
    }

    public override void MoveTo(PyramidFloorBoardElement element)
    {
        MoveGMCommand cmd = new MoveGMCommand(this, element);
        PlayerManager.Instance().local.HandleInput(cmd, true);
    }

    private void SetStartingElement(PyramidFloorBoardElement startElement)
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

    public virtual MeepleColor Color
    {
        get
        {
            return color;
        }
        set
        {
            color = value;
            texture = Texture4Color(value);

            SetStartingElement(Board.Instance().CornerPointsForColor(color)[meepIdx]);
            BackToStart();
        }
    }
}