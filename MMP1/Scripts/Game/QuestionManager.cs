using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class QuestionManager
{
    public delegate void QuestionAnswerdCallback(bool correct);

    public Dictionary<QuestionBoardElement,PyramidFloorBoardElement> questionElements { get; private set; }

    public bool isMovingQuestionBoardElement { get; private set; }
    private QuestionBoardElement movingQBE;

    private List<Question> questions;
    private Rectangle questionRect;
    private Random random;

    private KeyValuePair<QuestionBoardElement, Question> questionPair;

    private QuestionManager()
    {
        int margin = UnitConvert.ToAbsolute(20);
        Point size = new Point(Game1.windowHeight + margin * 2, Game1.windowHeight - margin);
        Point pos = new Point((Game1.windowWidth - size.X) / 2, margin / 2);
        questionRect = new Rectangle(pos, size);

        questions = new List<Question>();
        questions.Add(new QuestionKnowledge("Q1", QuestionKnowledge.fourAnswers, questionRect));

        random = new Random();

        questionElements = new Dictionary<QuestionBoardElement, PyramidFloorBoardElement>();
    }

    public void AskRandom(QuestionAnswerdCallback callback)
    {
        Question question = questions[random.Next(questions.Count)];
        question.callback = callback;
        question.Initiate();
    }
    
    public void AddQuestionBoardElement(QuestionBoardElement element, PyramidFloorBoardElement below)
    {
        questionElements.Add(element, below);
    }

    public void MoveQuestionElement(QuestionBoardElement element, PyramidFloorBoardElement to)
    {
        element.MoveTo(to.Position.Location);
        questionElements[element] = to;
    }

    public bool HasQuestionAbove(PyramidFloorBoardElement element)
    {
        return questionElements.ContainsValue(element);
    }

    public void ReceiveMouseInput(Point mousePos)
    {

    }

    public void InitiateQuestionBoardElementMove(QuestionBoardElement element)
    {
        isMovingQuestionBoardElement = true;
        // move piece with mouse
    }

    public void ClickedSomePyramidBoardElement(PyramidFloorBoardElement element)
    {
        // move and update stuff
    }

    private static QuestionManager manager;
    public static QuestionManager Instance()
    {
        if (manager == null) manager = new QuestionManager();
        return manager;
    }
}