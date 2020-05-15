using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class PlayButtonBoardElement : StaticVisibleBoardElement
{
    private string[] dissapearUIDs;

    public PlayButtonBoardElement(Rectangle position, Texture2D texture, string UID, int zPosition = 0, params string[] dissapearUIDs) : base(position, texture, UID, zPosition)
    {
        this.dissapearUIDs = dissapearUIDs;
    }

    public override void OnClick()
    {
        List<BoardElement> toRemove = new List<BoardElement>();
        toRemove.Add(this);
        foreach(string uid in dissapearUIDs)
        {
            BoardElement found = Board.Instance().FindByUID(uid);
            if(found != null) { toRemove.Add(found); }
        }

        CommandQueue.Queue(new RemoveFromBoardCommand(toRemove.ToArray()));
    }
}