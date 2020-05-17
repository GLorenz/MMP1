// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using System;

public class MoveQBECommand : INetworkCommand
{
    public const string name = "MV_QBE";

    protected QuestionBoardElement element;
    protected PyramidFloorBoardElement moveTo;

    public MoveQBECommand(QuestionBoardElement element, PyramidFloorBoardElement moveTo)
    {
        this.element = element;
        this.moveTo = moveTo;
    }

    public void Execute()
    {
        QuestionManager.Instance().MoveQuestionElementLocalOnly(element, moveTo);
    }

    public static MoveQBECommand FromInput(SerializableCommand sCommand)
    {
        try
        {
            QuestionBoardElement movingEl = (QuestionBoardElement)Board.Instance().FindByUID(sCommand.UID);
            PyramidFloorBoardElement floorEl = (PyramidFloorBoardElement)Board.Instance().FindByUID(sCommand.body);

            return new MoveQBECommand(movingEl, floorEl);
        }
        catch (InvalidCastException ice)
        {
            Console.WriteLine(ice.Message);
            return null;
        }
    }

    public SerializableCommand ToSerializable(bool shouldShare)
    {
        return new SerializableCommand(name, element.UID, moveTo.UID, shouldShare);
    }
}