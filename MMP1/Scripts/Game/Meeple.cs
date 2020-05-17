// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using System;

public class Meeple : GhostMeeple
{
    public new const int defaultZPosition = 11;
    public Player player { get; protected set; }

    public Meeple(Player player, PyramidFloorBoardElement standingOn, string UID, int zPosition = defaultZPosition, int meepidx = 0) : base(player, standingOn, UID, zPosition, meepidx)
    {
        this.player = player;
    }
    public Meeple(Player player, Rectangle position, string UID, int zPosition = defaultZPosition, int meepidx = 0) : base(player, position, UID, zPosition, meepidx)
    {
        this.player = player;
    }

    public override void MoveToLocalOnly(PyramidFloorBoardElement element)
    {
        base.MoveToLocalOnly(element);
        if (QuestionManager.Instance().HasQuestionAbove(element))
        {
            QuestionManager.Instance().AskRandom(OnQuestionAnswered);
        }
        if (element.Equals(Board.Instance().winningField))
        {
            GameOverCommand goc = new GameOverCommand(this);
            PlayerManager.Instance().local.HandleInput(goc, true);
        }
    }

    public override void Create()
    {
        CommandQueue.Queue(new AddToBoardCommand(this));
        Color = player.MeepleColor;
        Console.WriteLine("created meeple " + UID);
    }

    private void OnQuestionAnswered(bool correct)
    {
        if(correct)
        {
            QuestionManager.Instance().InitiateQuestionBoardElementMove(standingOn);
        }
        else
        {
            BackToStart();
        }
    }

    public override void OnClick()
    {
        QuickTimeMovement.Instance().Toggle(this);
    }

    public override MeepleColor Color
    {
        get
        {
            return base.Color;
        }
        set
        {
            startElement?.SetIsStartingField(false);
            base.Color = value;
            startElement.SetIsStartingField(true);
        }
    }
}