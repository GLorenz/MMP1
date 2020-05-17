using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class PyramidFloorBoardElement : NonMovingBoardElement, IVisibleBoardElement
{
    public List<PyramidFloorBoardElement> connectedFields { get; protected set; }
    private bool isStartingField;
    public bool isHoverTarget { get; protected set; }

    public PyramidFloorBoardElement(Rectangle position, string UID, int zPosition = 0) : base(position, TextureResources.Get("PyramidField"), UID, zPosition)
    {
        connectedFields = new List<PyramidFloorBoardElement>();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawDefault(spriteBatch);
    }

    public override void OnClick()
    {
        if(QuestionManager.Instance().isMovingQuestionBoardElement)
        {
            QuestionManager.Instance().ClickedSomePyramidBoardElement(this);
        }
    }

    public void AddConnectedField(PyramidFloorBoardElement field)
    {
        if(!connectedFields.Contains(field))
            connectedFields.Add(field);
    }

    public void SetIsStartingField(bool startingField)
    {
        isStartingField = startingField;
        TextureToDefault();
    }

    public void Hover()
    {
        texture = TextureResources.Get("PyramidFieldHover");
        isHoverTarget = true;
    }

    public void DeHover()
    {
        isHoverTarget = false;
        TextureToDefault();
    }

    private void TextureToDefault()
    {
        texture = TextureResources.Get(isStartingField ? "PyramidFieldStart" : "PyramidField");
    }
}