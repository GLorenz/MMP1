using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class PyramidLevel
{
    public enum Direction { NORTH, EAST, SOUTH, WEST }
    public List<FieldBoardElement> elements { get; protected set; }

    private delegate Point CreatePointFor(int i);

    private Point topLeft, bottomRight;
    private int fieldElemsCount;
    private string fieldTextureName;
    private bool hasDoubleCorner;

    private float xDistance, yDistance, start;
    private Point fieldSize;
    private Texture2D fieldTex;

    public PyramidLevel(Point topLeft, Point bottomRight, int fieldElemsCount, bool hasDoubleCorner = false, string fieldTextureName = "red")
    {
        this.topLeft = topLeft;
        this.bottomRight = bottomRight;
        this.fieldElemsCount = fieldElemsCount;
        this.fieldTextureName = fieldTextureName;
        this.hasDoubleCorner = hasDoubleCorner;

        elements = new List<FieldBoardElement>();

        CalcValues();
        Initiate();
    }

    private void CalcValues()
    {
        xDistance = (bottomRight.X - topLeft.X - (hasDoubleCorner ? 0 : 1)) / ((float)fieldElemsCount - (hasDoubleCorner ? 0.25f : 1));
        yDistance = (bottomRight.Y - topLeft.Y - (hasDoubleCorner ? 0 : 1)) / ((float)fieldElemsCount - (hasDoubleCorner ? 0.25f : 1));
        start = hasDoubleCorner ? (int)(xDistance / 2f * 0.75f) : 0;
        fieldSize = new Point(Rou(xDistance / 2), Rou(yDistance / 2));
        fieldTex = TextureResources.Get(fieldTextureName);
    }

    private void Initiate()
    {
        CreateFieldsInDirection(Direction.NORTH);
        CreateFieldsInDirection(Direction.EAST);
        CreateFieldsInDirection(Direction.SOUTH);
        CreateFieldsInDirection(Direction.WEST);

    }

    private void CreateFieldsInDirection(Direction direction)
    {
        CreatePointFor createPointFunc = DecideCreatePointForFunc(direction);
        int max = fieldElemsCount - (hasDoubleCorner ? 0 : 1);
        for (int i = 0; i < max; i++)
        {
            FieldBoardElement newElem = new FieldBoardElement(new Rectangle(createPointFunc(i), fieldSize), fieldTex, 1);
            elements.Add(newElem);
        }
    }
    
    private Point NorthPosFor(int i)
    {
        return new Point(topLeft.X + Rou(start + i * xDistance), topLeft.Y);
    }
    private Point EastPosFor(int i)
    {
        return new Point(bottomRight.X, bottomRight.Y - Rou(start + (i + (hasDoubleCorner ? 0 : 1)) * yDistance));
    }
    private Point SouthPosFor(int i)
    {
        return new Point(bottomRight.X - Rou(start + i * xDistance), bottomRight.Y);
    }
    private Point WestPosFor(int i)
    {
        return new Point(topLeft.X, topLeft.Y + Rou(start + (i + (hasDoubleCorner ? 0 : 1)) * yDistance));
    }

    private CreatePointFor DecideCreatePointForFunc(Direction dir)
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

    private int Rou(float f)
    {
        return (int)Math.Round(f);
    }
}