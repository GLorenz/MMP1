using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class PyramidFloorBoardElement : NonMovingBoardElement, IVisibleBoardElement
{
    public List<PyramidFloorBoardElement> connectedFields { get; protected set; }

    public PyramidFloorBoardElement(Rectangle position, int zPosition = 0, int UID = 0) : base(position, TextureResources.Get("PyramidField"), zPosition, UID)
    {
        connectedFields = new List<PyramidFloorBoardElement>();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawDefault(spriteBatch);
    }

    public override void OnClick()
    {
        string s = "";
        foreach(var x in connectedFields) { s+=x.UID+", "; }
        Console.WriteLine("i am idx {0}, connected to {1};",UID,s);
    }

    public void AddConnectedField(PyramidFloorBoardElement field)
    {
        if(!connectedFields.Contains(field))
            connectedFields.Add(field);
    }

    public void Highlight()
    {
        texture = TextureResources.Get("red");
    }

    public void Lowlight()
    {
        texture = TextureResources.Get("PyramidField");
    }
}