using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class Board : GenericBoardElementHolder<BoardElement>
{
    public Rectangle space { get; set; }

    private int boardUnitCount;
    public int boardUnit { get; protected set; }

    private PyramidFloor[] floors;
    private Rectangle[] floorRects;
    private Rectangle[] backgroundRects;
    private PyramidFloorBoardElement winningField;

    private List<PyramidFloorBoardElementConnector> visibleConnections;

    public Board() : base() {
        boardUnitCount = 15;

        floors = new PyramidFloor[3];
        floorRects = new Rectangle[4];
        backgroundRects = new Rectangle[4];

        visibleConnections = new List<PyramidFloorBoardElementConnector>();
    }

    public void BuildPyramidInSpace()
    {
        boardUnit = space.Width / boardUnitCount;
        
        // floor rects
        floorRects[0] = new Rectangle(1, 1, 13, 13);
        floorRects[1] = new Rectangle(3, 3, 9, 9);
        floorRects[2] = new Rectangle(5, 5, 5, 5);
        floorRects[3] = new Rectangle(7, 7, 1, 1);

        //winning outer & inner rect
        backgroundRects[0] = new Rectangle(0, 0, 15, 15);
        backgroundRects[1] = new Rectangle(2, 2, 11, 11);
        backgroundRects[2] = new Rectangle(4, 4, 7, 7);
        backgroundRects[3] = new Rectangle(6, 6, 3, 3);

        floors[0] = new PyramidFloorDoubleCorner(7, floorRects[0], 4);
        floors[1] = new PyramidFloor(5, floorRects[1], 5);
        floors[2] = new PyramidFloor(4, floorRects[2], 6);
        foreach(PyramidFloor floor in floors) { AddElement(floor.elements.ToArray()); }

        winningField = new PyramidFloorBoardElement(ToAbsolute(floorRects[3]), 1);
        AddElement(winningField);

        // setting background images
        for(int i = 1; i <= 4; i++)
        {
            string backgroundName = "PyramidBackgroundFloor" + i;
            StaticVisibleBoardElement background = new StaticVisibleBoardElement(ToAbsolute(backgroundRects[i-1]), TextureResources.Get(backgroundName), i != 4 ? i : 8);
            AddElement(background);
        }

        AddElevationConnections();
        AddRegularConnections();
        MakeConnectionsVisible();
    }

    public PyramidFloorBoardElement[] CornerPointsForColor(MeepleColor color)
    {
        int idx1, idx2;

        switch(color)
        {
            case MeepleColor.Red:
                idx1 = floors[0].cornerIndices[0];
                idx2 = floors[0].cornerIndices[1];
                break;
            case MeepleColor.White:
                idx1 = floors[0].cornerIndices[2];
                idx2 = floors[0].cornerIndices[3];
                break;
            case MeepleColor.Green:
                idx1 = floors[0].cornerIndices[4];
                idx2 = floors[0].cornerIndices[5];
                break;
            case MeepleColor.Black:
                idx1 = floors[0].cornerIndices[6];
                idx2 = floors[0].cornerIndices[7];
                break;
            default:
                idx1 = floors[0].cornerIndices[0];
                idx2 = floors[0].cornerIndices[1];
                break;
        }
        return new PyramidFloorBoardElement[] { floors[0].elements[idx1], floors[0].elements[idx2] };
    }

    protected void MakeConnectionsVisible()
    {
        AddElement(visibleConnections.ToArray());
    }

    protected void AddElevationConnections()
    {
        for(int i = 0; i < floors.Length; i++)
        {
            for(int f = 0; f < floors[i].elevationFromIndices.Count; f++)
            {
                PyramidFloorBoardElement from = floors[i].elements[floors[i].elevationFromIndices[f]];
                PyramidFloorBoardElement to;

                if (i + 1 < floors.Length)
                {
                    from = floors[i].elements[floors[i].elevationFromIndices[f]];
                    to = floors[i + 1].elements[floors[i + 1].elevationToIndices[f]];
                }
                else
                {
                    from = floors[i].elements[floors[i].elevationFromIndices[f]];
                    to = winningField;
                }

                to.AddConnectedField(from);
                from.AddConnectedField(to);
                visibleConnections.Add(new PyramidFloorBoardElementConnector(from, to, from.ZPosition - 1));
            }
        }
    }

    protected void AddRegularConnections()
    {
        foreach(PyramidFloor floor in floors)
        {
            for(int i = 0; i < floor.elements.Count; i++)
            {
                int next = i == floor.elements.Count - 1 ? 0 : i + 1;
                int past = i == 0 ? floor.elements.Count - 1 : i - 1;

                floor.elements[i].AddConnectedField(floor.elements[next]);
                floor.elements[i].AddConnectedField(floor.elements[past]);
                visibleConnections.Add(new PyramidFloorBoardElementConnector(floor.elements[i], floor.elements[next], floor.elements[i].ZPosition - 1));
            }
        }
    }

    public Rectangle ToAbsolute(Vector2 posInBU, Point sizeInBU)
    {
        return new Rectangle(space.X + Round(posInBU.X * boardUnit), space.Y + Round(posInBU.Y * boardUnit), sizeInBU.X * boardUnit, sizeInBU.Y * boardUnit);
    }

    public Rectangle ToAbsolute(Rectangle rectInBU)
    {
        return new Rectangle(space.X + rectInBU.X * boardUnit, space.Y + rectInBU.Y * boardUnit, rectInBU.Width * boardUnit, rectInBU.Height * boardUnit);
    }

    protected virtual int Round(float f)
    {
        return (int)Math.Round(f);
    }

    private static Board boardInst;
    public static Board Instance()
    {
        if (boardInst == null) { boardInst = new Board(); }
        return boardInst;
    }
}