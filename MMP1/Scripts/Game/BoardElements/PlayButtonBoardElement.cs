// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class PlayButtonBoardElement : AlphaAnimatedVisibleBoardElement
{
    private string[] dissapearUIDs;

    public PlayButtonBoardElement(Rectangle position, Texture2D texture, string UID, int zPosition = 0, params string[] dissapearUIDs) : base(position, texture, UID, zPosition)
    {
        this.dissapearUIDs = dissapearUIDs;
    }

    public override void OnClick()
    {
        List<BoardElement> toRemove = new List<BoardElement>();
        List<AlphaAnimatedVisibleBoardElement> toFadeOut = new List<AlphaAnimatedVisibleBoardElement>();
        toFadeOut.Add(this);
        foreach(string uid in dissapearUIDs)
        {
            BoardElement found = Board.Instance().FindByUID(uid);
            if(found != null) {
                if(found is AlphaAnimatedVisibleBoardElement)
                {
                    toFadeOut.Add((AlphaAnimatedVisibleBoardElement)found);
                }
                else
                {
                    toRemove.Add(found);
                }
            }
        }

        CommandQueue.Queue(new RemoveFromBoardCommand(toRemove.ToArray()));

        foreach(AlphaAnimatedVisibleBoardElement fadingElement in toFadeOut)
        {
            fadingElement.Fade(0f, RemoveFromBoard);
        }
    }

    private void RemoveFromBoard(AlphaAnimatedVisibleBoardElement alphaElement)
    {
        CommandQueue.Queue(new RemoveFromBoardCommand(alphaElement));
    }
}