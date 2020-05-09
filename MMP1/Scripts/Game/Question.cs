using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Question : StaticVisibleBoardElement
{
    private static readonly int defaultZPosition = 30;

    public delegate void QuestionAnswerdCallback(bool correct);
    protected QuestionAnswerdCallback callback;

    protected int margin;
    protected Rectangle contentRect;

    protected bool isConstructed;

    public Question(QuestionAnswerdCallback callback, Rectangle position, int UID = 0) 
        : base(position, TextureResources.Get("QuestionBackground"), defaultZPosition, UID)
    {
        this.callback = callback;
        isConstructed = false;

        margin = UnitConvert.ToAbsolute(20);
        contentRect = new Rectangle(position.X + margin, position.Y + margin, position.Width - margin * 2, position.Height - margin * 2);
    }

    protected virtual void OnAnswered(bool correct)
    {
        callback(correct);
        Exit();
    }

    public virtual void Construct()
    {
        // do nothing by default
    }

    public virtual void Initiate()
    {
        if(!isConstructed)
        {
            Construct();
            isConstructed = true;
        }
        Board.Instance().AddElement(this);
    }

    public virtual void Exit()
    {
        Board.Instance().RemoveElement(this);
    }
}