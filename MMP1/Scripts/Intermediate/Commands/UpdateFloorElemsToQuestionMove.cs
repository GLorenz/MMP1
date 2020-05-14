using System.Collections.Generic;

public class UpdateFloorElemsToQuestionMove : ICommand
{
    private List<PyramidFloorBoardElement> floorElements;
    private PyramidFloorBoardElement updated;
    private int index;

    public UpdateFloorElemsToQuestionMove(List<PyramidFloorBoardElement> floorElements, PyramidFloorBoardElement updated, int index)
    {
        this.floorElements = floorElements;
        this.updated = updated;
        this.index = index;
    }
    public void execute()
    {
        floorElements[index] = updated;
    }
}
