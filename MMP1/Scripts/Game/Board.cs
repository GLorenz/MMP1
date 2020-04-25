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

    private List<BoardElement> boardElements { get; set; }

    public BoardElement FindByIdentifier(string identifier)
    {
        return boardElements.Find(e => e.UID.Equals(identifier));
    }
}