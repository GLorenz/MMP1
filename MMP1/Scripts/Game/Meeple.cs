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

    public override void MoveTo(PyramidFloorBoardElement element)
    {
        base.MoveTo(element);
        if(QuestionManager.Instance().HasQuestionAbove(element))
        {
            QuestionManager.Instance().AskRandom(OnQuestionAnswered);
        }
        if(element.Equals(Board.Instance().winningField))
        {
            Game1.OnGameOver();
        }
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
}