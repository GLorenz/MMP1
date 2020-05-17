// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text;

public class Setupper
{
    private Rectangle boardRect;
    private int windowWidth, windowHeight;
    private SpriteFont[] fonts;

    public Setupper(int windowWidth, int windowHeight, params SpriteFont[] fonts)
    {
        this.windowWidth = windowWidth;
        this.windowHeight = windowHeight;
        this.fonts = fonts;
    }

    public void Setup()
    {
        SetupBackground("welcomebackground", 20);
        SetupWelcomeTitle();
        SetupPlayButton();

        SetupBackground("gamebackground", 0);
        SetupBoard();

        CreatePlayer();

        SetupNamePlates();
    }

    protected void SetupWelcomeTitle()
    {
        Texture2D titleTex = TextureResources.Get("LogoMixed");
        float aspectR = titleTex.Width / (float)titleTex.Height;
        int titleWidth = windowWidth * 3 / 4;
        int titleHeight = (int)(titleWidth / aspectR);
        int titleX = (windowWidth - titleWidth) / 2;
        int titleY = (windowHeight - titleHeight) / 16;
        StaticVisibleBoardElement title = new StaticVisibleBoardElement(new Rectangle(titleX, titleY, titleWidth, titleHeight), titleTex, "welcometitle", 21);

        CommandQueue.Queue(new AddToBoardCommand(title));
    }

    protected void SetupPlayButton()
    {
        Texture2D btnTex = TextureResources.Get("Play");
        float aspectR = btnTex.Width / (float)btnTex.Height;
        int btnWidth = UnitConvert.ToAbsoluteWidth(300);
        int btnHeight = (int)(btnWidth / aspectR);
        int btnX = (windowWidth - btnWidth) / 2;
        int btnY = (windowHeight - btnHeight) * 7 / 8;
        PlayButtonBoardElement playBtn = new PlayButtonBoardElement(new Rectangle(btnX, btnY, btnWidth, btnHeight), btnTex, "btn_play", 22, "welcomebackground", "welcometitle");

        CommandQueue.Queue(new AddToBoardCommand(playBtn));
    }

    protected void SetupBoard()
    {
        int boardHeight = windowHeight;
        int boardWidth = boardHeight;
        int boardX = (windowWidth - boardWidth) / 2;
        int boardY = (windowHeight - boardHeight) / 2;
        boardRect = new Rectangle(boardX, boardY, boardWidth, boardHeight);

        CommandQueue.Queue(new BuildPyramidCommand(boardRect));
    }

    protected void SetupBackground(string uid, int zPosition)
    {
        Texture2D background = TextureResources.Get("Background");
        float backgroundAspectRatio = background.Width / (float)background.Height;
        int backgroundWidth = windowWidth;
        int backgroundHeight = (int)(backgroundWidth / backgroundAspectRatio);
        int backgroundX = (windowWidth - backgroundWidth) / 2;
        int backgroundY = (windowHeight - backgroundHeight) / 2;
        StaticVisibleBoardElement backgroundBoardEl = new StaticVisibleBoardElement(new Rectangle(backgroundX, backgroundY, backgroundWidth, backgroundHeight), background, uid, zPosition: zPosition);
        CommandQueue.Queue(new AddToBoardCommand(backgroundBoardEl));
    }

    protected void CreatePlayer()
    {
        string name = NameList.GetRandomName();
        string uid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        Console.WriteLine("generated {0} and {1}", name, uid);
        CommandQueue.Queue(new CreatePlayerCommand(name, "player_" + uid));
    }

    protected void SetupNamePlates()
    {
        int margin = UnitConvert.ToAbsoluteWidth(20);
        Rectangle namePlateRectFoes = new Rectangle(boardRect.Right + margin, boardRect.Top + margin, ((windowWidth - boardRect.Width) / 2) - margin - margin, windowHeight / 2);
        NamePlateFoes namePlateFoes = new NamePlateFoes(namePlateRectFoes, fonts[0], fonts[1], "namePlateFoes", 1);

        Rectangle namePlateRectLocal = new Rectangle(margin, boardRect.Top + margin, ((windowWidth - boardRect.Width) / 2) - margin - margin, windowHeight / 4);
        NamePlateLocal namePlateLocal = new NamePlateLocal(namePlateRectLocal, fonts[0], fonts[1], "namePlateLocal", 1);

        PlayerManager.Instance().AddObserver(namePlateFoes);
        PlayerManager.Instance().AddObserver(namePlateLocal);
        CommandQueue.Queue(new AddToBoardCommand(namePlateFoes, namePlateLocal));
    }
}