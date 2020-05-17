using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class NamePlateFoes : StaticVisibleBoardElement, IObserver
{
    private static readonly Point margin = new Point(UnitConvert.ToAbsoluteWidth(20));
    private TextBoardElement foes, foesNames;
    private Rectangle contentRect;

    public NamePlateFoes(Rectangle position, SpriteFont fontSmall, SpriteFont fontBig, string UID, int zPosition = 0) : base(position, TextureResources.Get("NamePlateBackground"), UID, zPosition)
    {
        contentRect = new Rectangle(position.Location + margin, position.Size - margin - margin);

        Rectangle foesRect = new Rectangle(contentRect.Location.X, contentRect.Location.Y, contentRect.Width, contentRect.Height / 6);
        foes = new TextBoardElement(foesRect, "Your foes are:", fontSmall, "namePlateFoes", ColorResources.white, zPosition + 1, TextBoardElement.Alignment.LeftBottom);

        Rectangle foesNameRect = new Rectangle(contentRect.Location.X, contentRect.Location.Y + foesRect.Height, contentRect.Width, contentRect.Height * 5 / 6);
        foesNames = new TextBoardElement(foesNameRect, GetEnemiesNamesStr(), fontBig, "namePlateFoesNames", ColorResources.white, zPosition + 1, TextBoardElement.Alignment.LeftTop);

        CommandQueue.Queue(new AddToBoardCommand(foes, foesNames));
    }

    public void Update()
    {
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
            foesNamesStr += enemy.name + ",\n";
        }
        return foesNamesStr.Replace(GetLocalPlayerName()+",\n","").TrimEnd(' ',',','\n');
    }
}