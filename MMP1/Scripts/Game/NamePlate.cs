using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class NamePlate : StaticVisibleBoardElement, IObserver
{
    private static readonly Point margin = new Point(UnitConvert.ToAbsoluteWidth(20));
    private TextBoardElement hello, playerName, foes, foesNames;
    private Rectangle contentRect;

    public NamePlate(Rectangle position, SpriteFont fontSmall, SpriteFont fontBig, string UID, int zPosition = 0) : base(position, TextureResources.Get("NamePlateBackground"), UID, zPosition)
    {
        contentRect = new Rectangle(position.Location + margin, position.Size - margin - margin);

        Rectangle helloRect = new Rectangle(contentRect.Location.X, contentRect.Location.Y, contentRect.Width, contentRect.Height / 12);
        hello = new TextBoardElement(helloRect, "Hello", fontSmall, "namePlateHello", ColorResources.white, zPosition + 1, TextBoardElement.Alignment.LeftBottom);

        Rectangle nameRect = new Rectangle(contentRect.Location.X, contentRect.Location.Y + helloRect.Height, contentRect.Width, contentRect.Height / 6);
        playerName = new TextBoardElement(nameRect, GetLocalPlayerName()+"!", fontBig, "namePlatePlayer", ColorResources.white, zPosition + 1, TextBoardElement.Alignment.LeftTop);

        Rectangle foesRect = new Rectangle(contentRect.Location.X, contentRect.Location.Y + helloRect.Height + nameRect.Height, contentRect.Width, contentRect.Height / 12);
        foes = new TextBoardElement(foesRect, "Your foes are:", fontSmall, "namePlateFoes", ColorResources.white, zPosition + 1, TextBoardElement.Alignment.LeftBottom);

        Rectangle foesNameRect = new Rectangle(contentRect.Location.X, contentRect.Location.Y + helloRect.Height + nameRect.Height + foesRect.Height, contentRect.Width, contentRect.Height * 2 / 3);
        foesNames = new TextBoardElement(foesNameRect, GetEnemiesNamesStr(), fontBig, "namePlateFoesNames", ColorResources.white, zPosition + 1, TextBoardElement.Alignment.LeftTop);

        CommandQueue.Queue(new AddToBoardCommand(hello, playerName, foes, foesNames));
    }

    public void Update()
    {
        playerName.Text = GetLocalPlayerName()+"!";
        foesNames.Text = GetEnemiesNamesStr();
    }

    private string GetLocalPlayerName()
    {
        return PlayerManager.Instance().local?.name ?? "You";
    }

    private string GetEnemiesNamesStr()
    {
        string foesNamesStr = "";
        foreach (GhostPlayer enemy in PlayerManager.Instance().ghostPlayers)
        {
            foesNamesStr += enemy.name + ", ";
        }
        return foesNamesStr.Replace(GetLocalPlayerName()+", ","").TrimEnd(' ',',');
    }
}