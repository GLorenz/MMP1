using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class QuestionManager
{
    public delegate void QuestionAnswerdCallback(bool correct);

    // again, for bi-directional linking (instaed of dictionary)
    public List<QuestionBoardElement> questionElems { get; private set; }
    public List<PyramidFloorBoardElement> floorElems { get; private set; }

    public bool isMovingQuestionBoardElement { get; private set; }
    private QuestionBoardElement movingQBE;
    private Point movingQBEOffset;

    private List<Question> questions;
    private Rectangle questionRect;
    private Random random;

    public SpriteFont questionFont { get; protected set; }
    public SpriteFont answerFont { get; protected set; }

    private QuestionManager()
    {
        int margin = UnitConvert.ToAbsoluteWidth(20);
        Point size = new Point(Game1.windowHeight + margin * 2, Game1.windowHeight - margin);
        Point pos = new Point((Game1.windowWidth - size.X) / 2, margin / 2);
        questionRect = new Rectangle(pos, size);

        questions = new List<Question>();
        QuestionKnowledgeContent content = new QuestionKnowledgeContent("What is your favourite game?", 0, "Pyramid Paths Online", "Fortnite", "Call of Duty", "Apex Legends");
        questions.Add(new QuestionKnowledge(content, QuestionKnowledge.fourAnswers, questionRect, "questknow_" + content.question));

        random = new Random();

        questionElems = new List<QuestionBoardElement>();
        floorElems = new List<PyramidFloorBoardElement>();

        movingQBEOffset = new Point(1, 1);
    }

    public void AskRandom(QuestionAnswerdCallback callback)
    {
        Question question = questions[random.Next(questions.Count)];
        question.callback = callback;
        question.Initiate();
    }
    
    public void AddQuestionBoardElement(QuestionBoardElement element, PyramidFloorBoardElement below)
    {
        questionElems.Add(element);
        floorElems.Add(below);
    }

    public void MoveQuestionElement(QuestionBoardElement questionElement, PyramidFloorBoardElement floorElement)
    {
        questionElement.MoveTo(floorElement);

        CommandQueue.Queue(new UpdateFloorElemsToQuestionMove(floorElems, floorElement, questionElems.IndexOf(questionElement)));
    }

    public void MoveQuestionElementLocalOnly(QuestionBoardElement questionElement, PyramidFloorBoardElement floorElement)
    {
        questionElement.MoveToLocalOnly(floorElement);
        CommandQueue.Queue(new UpdateFloorElemsToQuestionMove(floorElems, floorElement, questionElems.IndexOf(questionElement)));
    }

    public bool HasQuestionAbove(PyramidFloorBoardElement element)
    {
        return floorElems.Contains(element);
    }

    public void ReceiveMouseInput(Point mousePos)
    {
        movingQBE.MoveTo(mousePos + movingQBEOffset);
    }

    public void InitiateQuestionBoardElementMove(PyramidFloorBoardElement floorElementBelow)
    {
        isMovingQuestionBoardElement = true;
        movingQBE = questionElems[floorElems.IndexOf(floorElementBelow)];
        movingQBE.SetZPosition(Board.Instance().MaxDepth + 1);
    }

    public void ClickedSomePyramidBoardElement(PyramidFloorBoardElement element)
    {
        isMovingQuestionBoardElement = false;
        MoveQuestionElement(movingQBE, element);
        movingQBE = null;
    }

    public void SetQuestionFont(SpriteFont font)
    {
        this.questionFont = font;
    }
    public void SetAnswerFont(SpriteFont font)
    {
        this.answerFont = font;
    }

    private static QuestionManager manager;
    public static QuestionManager Instance()
    {
        if (manager == null) manager = new QuestionManager();
        return manager;
    }
}