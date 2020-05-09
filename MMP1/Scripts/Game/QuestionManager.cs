using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class QuestionManager
{
    private List<Question> questions;
    private Rectangle questionRect;
    private Random random;

    private QuestionManager()
    {
        int margin = UnitConvert.ToAbsolute(20);
        Point size = new Point(Game1.windowHeight + margin * 2, Game1.windowHeight - margin);
        Point pos = new Point((Game1.windowWidth - size.X) / 2, margin / 2);
        questionRect = new Rectangle(pos, size);

        questions = new List<Question>();
        questions.Add(new QuestionKnowledge(OnQuestionAnswered, "Q1", QuestionKnowledge.fourAnswers, questionRect));

        random = new Random();
    }

    public void AskRandom()
    {
        questions[random.Next(questions.Count)].Initiate();
    }

    public void ReceiveMouseInput(Point mousePos)
    {

    }

    private void OnQuestionAnswered(bool correct)
    {
        Console.WriteLine("Answer is " + correct);
    }

    private static QuestionManager manager;
    public static QuestionManager Instance()
    {
        if (manager == null) manager = new QuestionManager();
        return manager;
    }
}