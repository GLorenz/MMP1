using System;
using System.Collections.Generic;

public class GenericBoardElementHolder<T_ELEM> where T_ELEM : BoardElement
{
    public List<T_ELEM> boardElements { get; protected set; }

    public GenericBoardElementHolder()
    {
        boardElements = new List<T_ELEM>();
    }

    public GenericBoardElementHolder(List<T_ELEM> elements)
    {
        boardElements = elements;
    }

    public void AddElement(T_ELEM element)
    {
        boardElements.Add(element);
    }

    public void RemoveElement(T_ELEM element)
    {
        boardElements.Remove(element);
    }

    public BoardElement FindByUID(int UID)
    {
        return boardElements.Find(e => e.UID.Equals(UID));
    }
}