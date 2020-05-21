// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class QuestionManager
{
    public delegate void QuestionAnswerdCallback(bool correct);

    private readonly string questionsXml = MMP1.Properties.Resources.questions;

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
        ParseQuestions();

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
        questionElement.MoveToAndShare(floorElement);

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
        movingQBE.MoveToLocalOnlyDirect(mousePos + movingQBEOffset);
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

    private void ParseQuestions()
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(questionsXml);
        Console.WriteLine(doc.FirstChild.Name);
        int questionIdx = 0;
        foreach (XmlNode qNode in doc.GetElementsByTagName("q"))
        {
            string title = "question";
            string[] answers = new string[4];
            int correct = 0;

            int.TryParse(qNode.Attributes["correct"].InnerText, out correct);
            title = qNode.Attributes["title"].InnerText;

            for (int i = 0; i < 4; i++)
            {
                answers[i] = qNode.ChildNodes[i].InnerText;
            }
            QuestionKnowledgeContent content = new QuestionKnowledgeContent(title, correct, answers);
            questions.Add(new QuestionKnowledge(content, QuestionKnowledge.fourAnswers, questionRect, "questknow_" + questionIdx));
            questionIdx++;
        }
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