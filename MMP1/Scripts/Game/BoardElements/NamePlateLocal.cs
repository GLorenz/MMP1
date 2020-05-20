// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class NamePlateLocal : StaticVisibleBoardElement, IObserver
{
    private static readonly Point margin = new Point(UnitConvert.ToAbsoluteWidth(20));
    private TextBoardElement hello, playerName;
    private Rectangle contentRect;

    public static NamePlateLocal current { get; private set; }

    public NamePlateLocal(Rectangle position, SpriteFont fontSmall, SpriteFont fontBig, string UID, int zPosition = 0) : base(position, TextureResources.Get("NamePlateBackgroundRed"), UID, zPosition)
    {
        current = this;
        contentRect = new Rectangle(position.Location + margin, position.Size - margin - margin);

        Rectangle helloRect = new Rectangle(contentRect.Location.X, contentRect.Location.Y, contentRect.Width, contentRect.Height / 4);
        hello = new TextBoardElement(helloRect, "Hello", fontSmall, "namePlateHello", ColorResources.White, zPosition + 1, TextBoardElement.Alignment.LeftBottom);

        Rectangle nameRect = new Rectangle(contentRect.Location.X, contentRect.Location.Y + helloRect.Height, contentRect.Width, contentRect.Height * 3 / 4);
        playerName = new TextBoardElement(nameRect, GetLocalPlayerName()+"!", fontBig, "namePlatePlayer", ColorResources.White, zPosition + 1, TextBoardElement.Alignment.LeftTop);

        CommandQueue.Queue(new AddToBoardCommand(hello, playerName));

        UpdateColor();
    }

    public void Update()
    {
        playerName.Text = GetLocalPlayerName()+"!";
    }

    public void UpdateColor()
    {
        string colorStr = PlayerManager.Instance().local.MeepleColor.ToString();
        texture = TextureResources.Get("NamePlateBackground" + colorStr);
        Color textColor = (colorStr.Equals("Red") || colorStr.Equals("Black")) ? ColorResources.White : ColorResources.Black;
        hello.SetColor(textColor);
        playerName.SetColor(textColor);
    }

    private string GetLocalPlayerName()
    {
        return PlayerManager.Instance().local?.name ?? "You";
    }
}