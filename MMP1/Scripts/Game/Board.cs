public class Board : GenericBoardElementHolder<BoardElement>
{
    private static Board boardInst;
    public static Board Instance()
    {
        if (boardInst == null) { boardInst = new Board(); }
        return boardInst;
    }

    public Board() : base() {
        
    }
}