using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class QuestionKnowledgeAnswer : StaticVisibleBoardElement
{
    private QuestionKnowledge.AnswerClickedCallback answerCallback;

    public QuestionKnowledgeAnswer(QuestionKnowledge.AnswerClickedCallback answerCallback, Rectangle position, Texture2D texture, string UID, int zPosition) 
        : base(position, texture, UID, zPosition)
    {
        this.answerCallback = answerCallback;
    }

    public override void OnClick()
    {
        answerCallback(this);
    }
}