﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

public class GenericBoardElementHolder<T_ELEM> where T_ELEM : BoardElement
{
    public List<T_ELEM> boardElements { get; protected set; }
    public List<IVisibleBoardElement> visibleElements { get; protected set; }
    private int maxDepth;
    public int MaxDepth {
        get
        {
            return Math.Max(1, maxDepth);
        }
        protected set {
            maxDepth = value;
        }
    }

    public GenericBoardElementHolder()
    {
        boardElements = new List<T_ELEM>();
        visibleElements = new List<IVisibleBoardElement>();
    }

    public GenericBoardElementHolder(List<T_ELEM> elements)
    {
        boardElements = elements;
    }

    public virtual void AddElement(params T_ELEM[] elements)
    {
        boardElements.AddRange(elements);
        UpdateUtils();
    }

    public virtual void RemoveElement(T_ELEM element)
    {
        boardElements.Remove(element);
        UpdateUtils();
    }

    private void UpdateUtils()
    {
        boardElements.Sort(BoardElement.CompareByZPosition);
        maxDepth = boardElements.AsQueryable().Select(e => e.ZPosition).Max();
        UpdateVisibleElements();
    }

    private void UpdateVisibleElements()
    {
        visibleElements = boardElements.AsQueryable().OfType<IVisibleBoardElement>().ToList();
        visibleElements.Sort((v1, v2) => v1.ZPosition.CompareTo(v2.ZPosition));
    }

    public BoardElement FindByUID(int UID)
    {
        return boardElements.Find(e => e.UID.Equals(UID));
    }

    public virtual void OnClick(Point pos)
    {
        boardElements.AsQueryable().Where(e => e.Position.Contains(pos)).Last().OnClick();
    }
}