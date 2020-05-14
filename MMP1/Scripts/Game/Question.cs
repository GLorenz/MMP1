using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public abstract class Question : StaticVisibleBoardElement
{
    private static readonly int defaultZPosition = 30;

    public QuestionManager.QuestionAnswerdCallback callback { get; set; }
    public QuestionBoardElement layingOn { get; set; }

    protected int margin;
    protected Rectangle contentRect;

    protected bool isConstructed;

    public Question(Rectangle position, string UID) 
        : base(position, TextureResources.Get("QuestionBackground"), UID, defaultZPosition)
    {
        isConstructed = false;

        margin = UnitConvert.ToAbsolute(20);
        contentRect = new Rectangle(position.X + margin, position.Y + margin, position.Width - margin * 2, position.Height - margin * 2);
    }

    protected virtual void OnAnswered(bool correct)
    {
        Exit();
        callback?.Invoke(correct);
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
        CommandQueue.Queue(new AddToBoardCommand(this));
        //Board.Instance().AddElement(this);
    }

    public virtual void Exit()
    {
        CommandQueue.Queue(new RemoveFromBoardCommand(this));
        //Board.Instance().RemoveElement(this);
    }
}