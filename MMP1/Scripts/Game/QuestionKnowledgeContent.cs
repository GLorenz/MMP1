// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

public class QuestionKnowledgeContent
{
    public string question { get; protected set; }
    public int correctIdx { get; protected set; }
    public string[] answers { get; protected set; }

    public QuestionKnowledgeContent(string question, int correctIdx, params string[] answers)
    {
        this.question = question;
        this.correctIdx = correctIdx;
        this.answers = answers;
    }
}