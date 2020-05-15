using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class QuestionKnowledgeAnswer : TextBoardElement
{
    private QuestionKnowledge.AnswerClickedCallback answerCallback;

    public QuestionKnowledgeAnswer(QuestionKnowledge.AnswerClickedCallback answerCallback, Rectangle position, string text, SpriteFont font, string UID, int margin, int zPosition = 0) 
        : base(new Rectangle(position.X+margin/2, position.Y+margin/2, position.Width-margin/2, position.Height-margin/2), text, font, UID, ColorResources.dark, zPosition)
    {
        this.answerCallback = answerCallback;
    }

    public override void OnClick()
    {
        answerCallback(this);
    }
}