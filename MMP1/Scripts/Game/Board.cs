using System.Collections.Generic;
using System.Linq;

public class Board
{
    private static Board boardInst;
    public static Board Instance()
    {
        if (boardInst == null) { boardInst = new Board(); }
        return boardInst;
    }
    private Board() { }

    public List<BoardElement> boardElements { get; private set; }

    public void AddElement(BoardElement element)
    {
        boardElements.Add(element);
    }

    public BoardElement FindByUID(string UID)
    {
        return boardElements.Find(e => e.UID.Equals(UID));
    }
}