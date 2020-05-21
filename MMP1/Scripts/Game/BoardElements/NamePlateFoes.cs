// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class NamePlateFoes : StaticVisibleBoardElement, IObserver
{
    private static readonly Point margin = new Point(UnitConvert.ToAbsoluteWidth(20));
    private TextBoardElement foes;
    private List<TextBoardElement> foesNames;
    private SpriteFont fontBig;
    private Rectangle contentRect;

    public static NamePlateFoes current { get; private set; }

    public NamePlateFoes(Rectangle position, SpriteFont fontSmall, SpriteFont fontBig, string UID, int zPosition = 0) : base(position, TextureResources.Get("NamePlateBackgroundLongRed"), UID, zPosition)
    {
        current = this;
        foesNames = new List<TextBoardElement>();
        this.fontBig = fontBig;
        contentRect = new Rectangle(position.Location + margin, position.Size - margin - margin);

        Rectangle foesRect = new Rectangle(contentRect.Location.X, contentRect.Location.Y, contentRect.Width, contentRect.Height / 6);
        foes = new TextBoardElement(foesRect, "Your foes are:", fontSmall, "namePlateFoes", ColorResources.White, zPosition + 1, TextBoardElement.Alignment.LeftBottom);
        
        CommandQueue.Queue(new AddToBoardCommand(foes));

        UpdateColor();
    }

    public void Update()
    {
        UpdateColor();
    }

    private string GetLocalPlayerName()
    {
        return PlayerManager.Instance().local?.name ?? "You";
    }

    public void UpdateColor()
    {
        CommandQueue.Queue(new RemoveFromBoardCommand(foesNames.ToArray()));
        BuildEnemies();
        CommandQueue.Queue(new AddToBoardCommand(foesNames.ToArray()));
    }

    private void BuildEnemies()
    {
        string localClrStr = PlayerManager.Instance().local.MeepleColor.ToString();
        texture = TextureResources.Get("NamePlateBackgroundLong" + localClrStr);
        foes.SetColor((localClrStr.Equals("Red") || localClrStr.Equals("Black")) ? ColorResources.White : ColorResources.Black);

        foesNames.Clear();
        int totalHeight = foes.Position.Height;
        foreach (GhostPlayer enemy in PlayerManager.Instance().ghostPlayers)
        {
            if (enemy.Equals(PlayerManager.Instance().local)) { continue; }
            Rectangle nextRect = new Rectangle(contentRect.Location.X, contentRect.Location.Y + totalHeight, contentRect.Width, contentRect.Height / 4);

            string colorStr = enemy.MeepleColor.ToString();
            Color textColor = ColorResources.ForName(colorStr);
            foesNames.Add(new TextBoardElement(nextRect, enemy.name, fontBig, "namePlateFoesNamesText"+enemy.name, textColor, zPosition + 2, TextBoardElement.Alignment.LeftMiddle));

            totalHeight += nextRect.Height;
        }
    }
}