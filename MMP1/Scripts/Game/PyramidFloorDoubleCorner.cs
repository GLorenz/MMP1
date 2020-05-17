// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class PyramidFloorDoubleCorner : PyramidFloor
{
    protected float offsetInBU;

    public PyramidFloorDoubleCorner(int fieldElemsCount, Rectangle spaceInBU, int elementsZPos) : base (fieldElemsCount, spaceInBU, elementsZPos) { }

    protected override void CalcValues()
    {
        offsetInBU = 1f;
        // todo: figure out
        strideInBU = (spaceInBU.Width) / (fieldElemsCount + 0.5f);
    }

    protected override void CreateFieldsInDirection(Direction direction)
    {
        CreatePointFor createPointFunc = DecideCreatePointForFunc(direction);
        int max = fieldElemsCount;
        for (int i = 0; i < max; i++)
        {
            PyramidFloorBoardElement newElem = new PyramidFloorBoardElement(Board.Instance().ToAbsolute(createPointFunc(i), fieldSizeInBU), "pyramidfloor" + fieldElemsCount + "_elem_" + direction.ToString() + i.ToString(), elementsZPos);
            elements.Add(newElem);
        }
    }

    protected override Vector2 NorthPosFor(int i)
    {
        return new Vector2(spaceInBU.Left + (i * strideInBU) + offsetInBU, spaceInBU.Top);
    }
    protected override Vector2 EastPosFor(int i)
    {
        return new Vector2(spaceInBU.Right - fieldSizeInBU.X, spaceInBU.Top + (i * strideInBU) + offsetInBU);
    }
    protected override Vector2 SouthPosFor(int i)
    {
        return new Vector2(spaceInBU.Right - (i * strideInBU) - offsetInBU, spaceInBU.Bottom) - fieldSizeInBU.ToVector2();
    }
    protected override Vector2 WestPosFor(int i)
    {
        return new Vector2(spaceInBU.Left, spaceInBU.Bottom - (i * strideInBU) - offsetInBU - fieldSizeInBU.Y);
    }

    public override void FillIndices()
    {
        bool evenElems = fieldElemsCount % 2 == 0;
        for (int i = 0; i < 4; i++)
        {
            int cur = i * fieldElemsCount;
            int prev = cur == 0 ? elements.Count - 1 : cur - 1;

            cornerIndices.Add(cur);
            cornerIndices.Add(prev);

            elevationFromIndices.Add((cur) + ((fieldElemsCount - 1) / 2));
        }
    }
}