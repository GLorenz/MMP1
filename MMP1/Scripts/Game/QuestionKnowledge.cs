﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class QuestionKnowledge : Question
{
    public static readonly int fourAnswers = 4;
    public static readonly int nineAnswers = 9;

    StaticVisibleBoardElement question;
    List<QuestionKnowledgeAnswer> answers;

    public delegate void AnswerClickedCallback(QuestionKnowledgeAnswer answer);
    private int correctAnswerIdx;

    private string questionName;
    private int answerCount;

    public QuestionKnowledge(string questionName, int answerCount, Rectangle position, string UID)
        : base(position, UID)
    {
        this.questionName = questionName;
        this.answerCount = answerCount;
    }

    public override void Construct()
    {
        base.Construct();
        answers = new List<QuestionKnowledgeAnswer>();
        correctAnswerIdx = 0;

        Texture2D qTex = TextureResources.Get(questionName);
        Rectangle qRect = new Rectangle(contentRect.X, contentRect.Y, contentRect.Width, contentRect.Width / (qTex.Width / qTex.Height));
        question = new StaticVisibleBoardElement(qRect, qTex, "questknow"+questionName+"title", ZPosition + 1);

        int gridSize = (int)Math.Sqrt(answerCount);
        Point answerSize = new Point(contentRect.Width / gridSize, contentRect.Height / (2*gridSize));
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                string ansTexName = questionName + ((char)(65 + x + y));

                Texture2D ansTex = TextureResources.Get(ansTexName);
                Rectangle ansRect = new Rectangle(
                    new Point(
                        contentRect.Left + (x % gridSize) * answerSize.X,
                        contentRect.Center.Y + (y % gridSize) * answerSize.Y
                    ),
                    answerSize
                );
                answers.Add(new QuestionKnowledgeAnswer(OnAnswerClicked, ansRect, ansTex, UID+ansTexName, zPosition + 1));
            }
        }
    }
    public void OnAnswerClicked(QuestionKnowledgeAnswer answer)
    {
        OnAnswered(answer == answers[correctAnswerIdx]);
    }

    public override void Initiate()
    {
        base.Initiate();
        CommandQueue.Queue(new AddToBoardCommand(question));
        CommandQueue.Queue(new AddToBoardCommand(answers.ToArray()));
    }

    public override void Exit()
    {
        CommandQueue.Queue(new RemoveFromBoardCommand(question));
        CommandQueue.Queue(new RemoveFromBoardCommand(answers.ToArray()));
        base.Exit();
    }
}