using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

public class Board : GenericBoardElementHolder<BoardElement>
{
    public Rectangle space { get; set; }

    private int boardUnitCount;
    private int boardUnit;

    private PyramidFloor[] floors;
    private Rectangle[] floorRects;
    private Rectangle[] backgroundRects;
    private PyramidFloorBoardElement winningField;

    public Board() : base() {
        boardUnitCount = 15;
        floors = new PyramidFloor[3];
        floorRects = new Rectangle[4];
        backgroundRects = new Rectangle[4];
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

        floors[0] = new PyramidFloorDoubleCorner(7, floorRects[0], 3);
        floors[1] = new PyramidFloor(5, floorRects[1], 4);
        floors[2] = new PyramidFloor(4, floorRects[2], 5);
        foreach(PyramidFloor floor in floors) { AddElement(floor.elements.ToArray()); }

        winningField = new PyramidFloorBoardElement(ToAbsolute(floorRects[3]), 1);
        AddElement(winningField);

        // setting background images
        for(int i = 1; i <= 4; i++)
        {
            string backgroundName = "PyramidBackgroundFloor" + i;
            StaticVisibleBoardElement background = new StaticVisibleBoardElement(ToAbsolute(backgroundRects[i-1]), TextureResources.Get(backgroundName), i);
            AddElement(background);
        }

        AddElevationConnections();
        AddRegularConnections();
        MakeConnectionsVisible();
    }

    protected void MakeConnectionsVisible()
    {
        var connectors = new List<PyramidFloorBoardElementConnector>();
        foreach (PyramidFloor floor in floors)
        {
            foreach (PyramidFloorBoardElement element in floor.elements)
            {
                foreach(PyramidFloorBoardElement connected in element.connectedFields)
                {
                    connectors.Add(new PyramidFloorBoardElementConnector(element, connected, element.ZPosition-1));
                }
            }
        }
        AddElement(connectors.ToArray());
    }

    protected void AddElevationConnections()
    {
        for(int i = 0; i < floors.Length; i++)
        {
            for(int f = 0; f < floors[i].elevationFromIndices.Count; f++)
            {
                if (i + 1 < floors.Length)
                {
                    PyramidFloorBoardElement from = floors[i].elements[floors[i].elevationFromIndices[f]];
                    PyramidFloorBoardElement to = floors[i + 1].elements[floors[i + 1].elevationToIndices[f]];
                    to.AddConnectedField(from);
                    from.AddConnectedField(to);
                }
                else
                {
                    floors[i].elements[floors[i].elevationFromIndices[f]].AddConnectedField(winningField);
                }
            }
        }
    }

    protected void AddRegularConnections()
    {
        foreach(PyramidFloor floor in floors)
        {
            for(int i = 0; i < floor.elements.Count; i++)
            {
                if(i != floor.elements.Count - 1)
                {
                    floor.elements[i].AddConnectedField(floor.elements[i + 1]);
                }
                else
                {
                    floor.elements[i].AddConnectedField(floor.elements[0]);
                }
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