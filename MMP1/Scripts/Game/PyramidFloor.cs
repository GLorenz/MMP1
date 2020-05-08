using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class PyramidFloor
{
    public enum Direction { NORTH, EAST, SOUTH, WEST }
    public List<PyramidFloorBoardElement> elements { get; protected set; }
    public List<int> cornerIndices { get; protected set; }
    public List<int> elevationFromIndices { get; protected set; }
    public List<int> elevationToIndices { get; protected set; }

    protected delegate Vector2 CreatePointFor(int i);

    protected Rectangle spaceInBU;
    protected int fieldElemsCount;

    protected float strideInBU;
    protected Point fieldSizeInBU;

    protected int elementsZPos;

    public PyramidFloor(int fieldElemsCount, Rectangle spaceInBU, int elementsZPos)
    {
        this.spaceInBU = spaceInBU;
        this.fieldElemsCount = fieldElemsCount;
        this.elementsZPos = elementsZPos;

        fieldSizeInBU = new Point(1, 1);
        cornerIndices = new List<int>();
        elevationFromIndices = new List<int>();
        elevationToIndices = new List<int>();

        elements = new List<PyramidFloorBoardElement>();
        Initiate();
    }

    protected virtual void Initiate()
    {
        CalcValues();

        CreateFieldsInDirection(Direction.NORTH);
        CreateFieldsInDirection(Direction.EAST);
        CreateFieldsInDirection(Direction.SOUTH);
        CreateFieldsInDirection(Direction.WEST);

        FillIndices();
    }

    protected virtual void CalcValues()
    {
        strideInBU = (spaceInBU.Width - 1f) / (fieldElemsCount - 1f);
    }

    protected virtual void CreateFieldsInDirection(Direction direction)
    {
        CreatePointFor createPointFunc = DecideCreatePointForFunc(direction);
        int max = fieldElemsCount - 1;
        for (int i = 0; i < max; i++)
        {
            PyramidFloorBoardElement newElem = new PyramidFloorBoardElement(Board.Instance().ToAbsolute(createPointFunc(i), fieldSizeInBU), elementsZPos, elements.Count);
            elements.Add(newElem);
        }
    }
    
    protected virtual Vector2 NorthPosFor(int i)
    {
        return new Vector2(spaceInBU.Left + (i * strideInBU), spaceInBU.Top);
    }
    protected virtual Vector2 EastPosFor(int i)
    {
        return new Vector2(spaceInBU.Right - fieldSizeInBU.X, spaceInBU.Top + (i * strideInBU));
    }
    protected virtual Vector2 SouthPosFor(int i)
    {
        return new Vector2(spaceInBU.Right - (i * strideInBU), spaceInBU.Bottom) - fieldSizeInBU.ToVector2();
    }
    protected virtual Vector2 WestPosFor(int i)
    {
        return new Vector2(spaceInBU.Left, spaceInBU.Bottom - (i * strideInBU) - fieldSizeInBU.Y);
    }

    protected virtual CreatePointFor DecideCreatePointForFunc(Direction dir)
    {
        switch (dir)
        {
            case Direction.NORTH:
                return NorthPosFor;
            case Direction.EAST:
                return EastPosFor;
            case Direction.SOUTH:
                return SouthPosFor;
            case Direction.WEST:
                return WestPosFor;
            default:
                return NorthPosFor;
        }
    }

    public virtual void FillIndices()
    {
        bool evenElems = fieldElemsCount % 2 == 0;
        for (int i = 0; i < 4; i++)
        {
            cornerIndices.Add(i * (fieldElemsCount-1));
            if(evenElems)
            {
                elevationFromIndices.Add((i * (fieldElemsCount - 1)) + (fieldElemsCount / 2));
                elevationFromIndices.Add((i * (fieldElemsCount - 1)) + (fieldElemsCount / 2) - 1);
            }
            else
            {
                elevationToIndices.Add((i * (fieldElemsCount - 1)) + ((fieldElemsCount - 1) / 2));
            }
        }

        if (evenElems)
        {
            elevationToIndices = cornerIndices;
        }
        else
        {
            elevationFromIndices = cornerIndices;
        }
    }
}