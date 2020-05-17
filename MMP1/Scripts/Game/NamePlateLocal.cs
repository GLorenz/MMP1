using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class NamePlateLocal : StaticVisibleBoardElement, IObserver
{
    private static readonly Point margin = new Point(UnitConvert.ToAbsoluteWidth(20));
    private TextBoardElement hello, playerName;
    private Rectangle contentRect;

    public NamePlateLocal(Rectangle position, SpriteFont fontSmall, SpriteFont fontBig, string UID, int zPosition = 0) : base(position, TextureResources.Get("NamePlateBackgroundSmall"), UID, zPosition)
    {
        contentRect = new Rectangle(position.Location + margin, position.Size - margin - margin);

        Rectangle helloRect = new Rectangle(contentRect.Location.X, contentRect.Location.Y, contentRect.Width, contentRect.Height / 4);
        hello = new TextBoardElement(helloRect, "Hello", fontSmall, "namePlateHello", ColorResources.white, zPosition + 1, TextBoardElement.Alignment.LeftBottom);

        Rectangle nameRect = new Rectangle(contentRect.Location.X, contentRect.Location.Y + helloRect.Height, contentRect.Width, contentRect.Height * 3 / 4);
        playerName = new TextBoardElement(nameRect, GetLocalPlayerName()+"!", fontBig, "namePlatePlayer", ColorResources.white, zPosition + 1, TextBoardElement.Alignment.LeftTop);

        CommandQueue.Queue(new AddToBoardCommand(hello, playerName));
    }

    public void Update()
    {
        playerName.Text = GetLocalPlayerName()+"!";
    }

    private string GetLocalPlayerName()
    {
        return PlayerManager.Instance().local?.name ?? "You";
    }
}