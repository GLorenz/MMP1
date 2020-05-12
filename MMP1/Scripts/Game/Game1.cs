using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class Game1 : Game
{
    public enum NetworkType { Online, Offline, Local }
    public static NetworkType networkType = NetworkType.Online;

    public static int playerCount = 4;
    public static int meepleCount = 2;

    public static int windowWidth { get; private set; }
    public static int windowHeight { get; private set; }
    public Rectangle boardRect { get; private set; }
    
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    // SpriteFont oldenburgFont;

    bool pressHandled;

    // c# says no, when using class without generics, and a simple downcast isn't possible :(
    GenericBoardElementHolder<BoardElement> elementsHolder;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        elementsHolder = Board.Instance();
    }

    protected override void Initialize()
    {
        // setting up display size here, since graphics aren't initialized in constructor
        windowWidth = GraphicsDevice.DisplayMode.Width - 100;
        windowHeight = GraphicsDevice.DisplayMode.Height - 100;
        Window.IsBorderless = false;
        Window.Position = new Point(50,50);
        graphics.PreferredBackBufferWidth = windowWidth;
        graphics.PreferredBackBufferHeight = windowHeight;
        IsMouseVisible = true;
        graphics.ApplyChanges();

        UnitConvert.screenWidth = windowWidth;
        UnitConvert.screenHeight = windowHeight;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        TextureResources.contentManager = Content;

        TextureResources.LoadDefault();

        // oldenburgFont = Content.Load<SpriteFont>("fonts/arial_16_bold");
        PlaceContent();
    }

    protected void PlaceContent()
    {
        SetupBoard();
        SetupBackground();

        CreatePlayer();
        CreateMeeples();
    }

    protected void SetupBoard()
    {
        int boardHeight = windowHeight;
        int boardWidth = boardHeight;
        int boardX = (windowWidth - boardWidth) / 2;
        int boardY = (windowHeight - boardHeight) / 2;
        boardRect = new Rectangle(boardX, boardY, boardWidth, boardHeight);

        Board.Instance().space = boardRect;
        Board.Instance().BuildPyramidInSpace();
    }

    protected void SetupBackground()
    {
        Texture2D background = TextureResources.Get("Background");
        float backgroundAspectRatio = background.Width / (float)background.Height;
        int backgroundWidth = windowWidth;
        int backgroundHeight = (int)(backgroundWidth / backgroundAspectRatio);
        int backgroundX = (windowWidth - backgroundWidth) / 2;
        int backgroundY = (windowHeight - backgroundHeight) / 2;
        StaticVisibleBoardElement backgroundBoardEl = new StaticVisibleBoardElement(new Rectangle(backgroundX, backgroundY, backgroundWidth, backgroundHeight), background, "gamebackground", zPosition: 0);
        Board.Instance().AddElement(backgroundBoardEl);
    }

    protected void CreatePlayer()
    {
        Player pRalph = new Player("Ralph", "player_ralph");
        PlayerManager.Instance().SetLocal(pRalph);

        CreateGhostPlayerCommand ghostCmd = new CreateGhostPlayerCommand(pRalph);
        PlayerManager.Instance().local.OnlyShare(ghostCmd);
    }

    protected void CreateMeeples()
    {
        MeepleColor color = MeepleColorClaimer.Next();
        PyramidFloorBoardElement[] cornerFields = Board.Instance().CornerPointsForColor(color);
        for (int i = 0; i < meepleCount; i++)
        {
            Meeple newMeep = new Meeple(PlayerManager.Instance().local, cornerFields[i].Position, color, "player"+PlayerManager.Instance().local.name+"_meeple"+i, 10);
            newMeep.SetStartingElement(cornerFields[i]);
            newMeep.BackToStart();
            elementsHolder.AddElement(newMeep);

            CreateGhostMeepleCommand meepCmd = new CreateGhostMeepleCommand(newMeep);
            PlayerManager.Instance().local.OnlyShare(meepCmd);
        }
    }
    
    protected override void UnloadContent()
    {
        // Unload any non ContentManager content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        if(QuestionManager.Instance().isMovingQuestionBoardElement)
        {
            QuestionManager.Instance().ReceiveMouseInput(Mouse.GetState().Position);
        }

        if (QuickTimeMovement.Instance().isActive)
        {
            QuickTimeMovement.Instance().ReceiveMousePos(Mouse.GetState().Position);
        }

        if(Mouse.GetState().LeftButton == ButtonState.Pressed && !pressHandled)
        {
            Board.Instance().OnClick(Mouse.GetState().Position);

            // do after, no time to explain
            if (QuickTimeMovement.Instance().isActive)
            {
                QuickTimeMovement.Instance().OnClick();
            }
            pressHandled = true;
        }
        else if (Mouse.GetState().LeftButton == ButtonState.Released && pressHandled) {
            pressHandled = false;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin(SpriteSortMode.Immediate);

        for (int i = 0; i < elementsHolder.visibleElements.Count; i++)
        {
            elementsHolder.visibleElements[i].Draw(spriteBatch);

            //spriteBatch.DrawString(oldenburgFont, "Enemys " + PlayerManager.Instance().ghostPlayers.Count, new Vector2(1500f, 100f), Color.White);
        }
        spriteBatch.End();

        base.Draw(gameTime);
    }

    public static void Main()
    {
        using (var game = new Game1())
            game.Run();
    }

    public static void OnGameOver()
    {
        Console.WriteLine("game over");
    }

    protected override void OnExiting(object sender, EventArgs args)
    {
        PlayerManager.Instance().local.DisconnectClient();
        base.OnExiting(sender, args);
    }
}
