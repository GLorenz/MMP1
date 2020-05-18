// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class QuestionKnowledge : Question
{
    public static readonly int fourAnswers = 4;
    public static readonly int nineAnswers = 9;

    public delegate void AnswerClickedCallback(QuestionKnowledgeAnswer answer);

    private QuestionKnowledgeContent content;

    private TextBoardElement question;
    private List<QuestionKnowledgeAnswer> answers;
    private List<StaticVisibleBoardElement> backgrounds;

    private int answerCount;

    public QuestionKnowledge(QuestionKnowledgeContent content, int answerCount, Rectangle position, string UID)
        : base(position, UID)
    {
        this.content = content;
        this.answerCount = answerCount;
    }

    public override void Construct()
    {
        base.Construct();
        answers = new List<QuestionKnowledgeAnswer>();
        backgrounds = new List<StaticVisibleBoardElement>();

        Rectangle qRect = new Rectangle(contentRect.X, contentRect.Y, contentRect.Width, contentRect.Height / 2);
        question = new TextBoardElement(qRect, content.question, QuestionManager.Instance().questionFont, "questknow_cur_title", ColorResources.dark, ZPosition + 2);
        backgrounds.Add(new StaticVisibleBoardElement(qRect, TextureResources.Get("WhiteBackground"), "questknow_cur_title_background", ZPosition + 1));

        int gridSize = (int)Math.Sqrt(answerCount);
        Point answerSize = new Point(contentRect.Width / gridSize, contentRect.Height / (2*gridSize));
        Point answerMargin = new Point(margin/answerCount, margin/gridSize);

        int charInt = 65;
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                Rectangle ansRect = new Rectangle(
                    new Point(
                        contentRect.Left + (x % gridSize) * (answerSize.X + margin/answerCount),
                        contentRect.Center.Y + (y % gridSize) * answerSize.Y + margin/gridSize
                    ),
                    answerSize - answerMargin
                );

                char answerChar = (char)charInt;
                string answerText = answerChar + ". " + content.answers[x + y*gridSize];
                string answerUID = UID + "answer" + x.ToString() + y.ToString();

                answers.Add(new QuestionKnowledgeAnswer(OnAnswerClicked, ansRect, answerText, QuestionManager.Instance().answerFont, answerUID, margin*3/5, zPosition + 2));
                backgrounds.Add(new StaticVisibleBoardElement(ansRect, TextureResources.Get("BorderBackground"), answerUID+"background", ZPosition + 1));

                charInt++;
            }
        }
    }
    public void OnAnswerClicked(QuestionKnowledgeAnswer answer)
    {
        OnAnswered(answer == answers[content.correctIdx]);
    }

    public override void Initiate()
    {
        base.Initiate();
        CommandQueue.Queue(new AddToBoardCommand(question));
        CommandQueue.Queue(new AddToBoardCommand(answers.ToArray()));
        CommandQueue.Queue(new AddToBoardCommand(backgrounds.ToArray()));
    }

    public override void Exit()
    {
        CommandQueue.Queue(new RemoveFromBoardCommand(question));
        CommandQueue.Queue(new RemoveFromBoardCommand(answers.ToArray()));
        CommandQueue.Queue(new RemoveFromBoardCommand(backgrounds.ToArray()));
        base.Exit();
    }
}