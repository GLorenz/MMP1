using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class QuestionKnowledgeAnswer : StaticVisibleBoardElement
{
    private QuestionKnowledge.AnswerClickedCallback answerCallback;

    public QuestionKnowledgeAnswer(QuestionKnowledge.AnswerClickedCallback answerCallback, Rectangle position, Texture2D texture, int zPosition, int UID = 0) 
        : base(position, texture, zPosition, UID)
    {
        this.answerCallback = answerCallback;
    }

    public override void OnClick()
    {
        answerCallback(this);
    }
}