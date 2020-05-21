// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Setupper
{
    private Rectangle boardRect;
    private int windowWidth, windowHeight;
    
    public Setupper(int windowWidth, int windowHeight)
    {
        this.windowWidth = windowWidth;
        this.windowHeight = windowHeight;
    }

    public void Setup()
    {
        SetupBackground("welcomebackground", 20);
        SetupWelcomeTitle();
        SetupPlayButton();
        SetupImpressum();

        SetupBackground("gamebackground", 0);
        SetupBoard();

        CreatePlayer();

        SetupNamePlates();
        NamePlateLocal.current.UpdateColor();
    }

    public void GameOver(string winnerName)
    {
        SetupBackground("gameoverbackground", 25, 0f).Fade(1f, null);
        SetupGameOverText(winnerName);
    }

    protected void SetupGameOverText(string winnerName)
    {
        Rectangle goRect = new Rectangle(0, 0, windowWidth, windowHeight);
        TextBoardElement go = new TextBoardElement(goRect, "Game Over! " + winnerName + " won!", FontResources.oldenburg_60, "gameovertext", ColorResources.Black, 26);
        CommandQueue.Queue(new AddToBoardCommand(go));
    }

    protected void SetupWelcomeTitle()
    {
        Texture2D titleTex = TextureResources.Get("LogoMixed");
        float aspectR = titleTex.Width / (float)titleTex.Height;
        int titleWidth = windowWidth * 3 / 4;
        int titleHeight = (int)(titleWidth / aspectR);
        int titleX = (windowWidth - titleWidth) / 2;
        int titleY = (windowHeight - titleHeight) / 16;
        AlphaAnimatedVisibleBoardElement title = new AlphaAnimatedVisibleBoardElement(new Rectangle(titleX, titleY, titleWidth, titleHeight), titleTex, "welcometitle", 21);

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
        PlayButtonBoardElement playBtn = new PlayButtonBoardElement(new Rectangle(btnX, btnY, btnWidth, btnHeight), btnTex, "btn_play", 22, "welcomebackground", "welcometitle", "impressum");

        CommandQueue.Queue(new AddToBoardCommand(playBtn));
    }

    protected void SetupImpressum()
    {
        string text = "Impressum: Developed by Lorenz Gonsa, FHS MMT for MultiMediaProject 1";
        SpriteFont font = FontResources.oldenburg_8;
        Vector2 textSize = font.MeasureString(text);
        Rectangle impressRect = new Rectangle(windowWidth - (int)textSize.X, windowHeight - (int)textSize.Y, (int)textSize.X + 10, (int)textSize.Y);
        TextBoardElement impressum = new TextBoardElement(impressRect, text, font, "impressum", ColorResources.Black, 21, TextBoardElement.Alignment.LeftTop);

        CommandQueue.Queue(new AddToBoardCommand(impressum));
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

    protected AlphaAnimatedVisibleBoardElement SetupBackground(string uid, int zPosition, float alpha = 1f)
    {
        Texture2D background = TextureResources.Get("Background");
        float backgroundAspectRatio = background.Width / (float)background.Height;
        int backgroundWidth = windowWidth;
        int backgroundHeight = (int)(backgroundWidth / backgroundAspectRatio);
        int backgroundX = (windowWidth - backgroundWidth) / 2;
        int backgroundY = (windowHeight - backgroundHeight) / 2;
        AlphaAnimatedVisibleBoardElement backgroundBoardEl = new AlphaAnimatedVisibleBoardElement(new Rectangle(backgroundX, backgroundY, backgroundWidth, backgroundHeight), background, uid, zPosition: zPosition, startAlpha: alpha);
        CommandQueue.Queue(new AddToBoardCommand(backgroundBoardEl));
        return backgroundBoardEl;
    }

    protected void CreatePlayer()
    {
        string name = NameList.GetRandomName();
        string uid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        CommandQueue.Queue(new CreatePlayerCommand(name, "player_" + uid));
    }

    protected void SetupNamePlates()
    {
        int margin = UnitConvert.ToAbsoluteWidth(20);
        Rectangle namePlateRectFoes = new Rectangle(boardRect.Right + margin, boardRect.Top + margin, ((windowWidth - boardRect.Width) / 2) - margin - margin, windowHeight / 2);
        NamePlateFoes namePlateFoes = new NamePlateFoes(namePlateRectFoes, FontResources.oldenburg_20, FontResources.oldenburg_30, "namePlateFoes", 1);

        Rectangle namePlateRectLocal = new Rectangle(margin, boardRect.Top + margin, ((windowWidth - boardRect.Width) / 2) - margin - margin, windowHeight / 4);
        NamePlateLocal namePlateLocal = new NamePlateLocal(namePlateRectLocal, FontResources.oldenburg_20, FontResources.oldenburg_30, "namePlateLocal", 1);

        PlayerManager.Instance().AddObserver(namePlateFoes);
        PlayerManager.Instance().AddObserver(namePlateLocal);

        MeepleColorClaimer.Instance().AddObserver(namePlateFoes);
        MeepleColorClaimer.Instance().AddObserver(namePlateLocal);

        CommandQueue.Queue(new AddToBoardCommand(namePlateFoes, namePlateLocal));
    }
}