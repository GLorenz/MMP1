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

    SpriteFont oldenburg_40, oldenburg_80, josefin_20;

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

        oldenburg_40 = Content.Load<SpriteFont>("fonts/oldenburg_40");
        oldenburg_80 = Content.Load<SpriteFont>("fonts/oldenburg_80");
        josefin_20 = Content.Load<SpriteFont>("fonts/josefin_20");

        PlaceContent();
    }

    protected void PlaceContent()
    {
        SetupBoard();
        SetupBackground();

        CreatePlayer();
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

    protected void SetupBackground()
    {
        Texture2D background = TextureResources.Get("Background");
        float backgroundAspectRatio = background.Width / (float)background.Height;
        int backgroundWidth = windowWidth;
        int backgroundHeight = (int)(backgroundWidth / backgroundAspectRatio);
        int backgroundX = (windowWidth - backgroundWidth) / 2;
        int backgroundY = (windowHeight - backgroundHeight) / 2;
        StaticVisibleBoardElement backgroundBoardEl = new StaticVisibleBoardElement(new Rectangle(backgroundX, backgroundY, backgroundWidth, backgroundHeight), background, "gamebackground", zPosition: 0);
        CommandQueue.Queue(new AddToBoardCommand(backgroundBoardEl));
    }

    protected void CreatePlayer()
    {
        string name = NameList.GetRandomName();
        string uid = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
        Console.WriteLine("generated {0} and {1}", name, uid);
        CommandQueue.Queue(new CreatePlayerCommand(name, "player_" + uid));
    }
    
    protected override void UnloadContent()
    {
        // Unload any non ContentManager content here
    }
    
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if(Keyboard.GetState().IsKeyDown(Keys.L))
        {
            PlayerManager.Instance().local.HandleInput(new LulzCommand(),true);
        }
        //else if (l && Keyboard.GetState().IsKeyUp(Keys.L)) { l = false; }

        if (QuestionManager.Instance().isMovingQuestionBoardElement)
        {
            QuestionManager.Instance().ReceiveMouseInput(Mouse.GetState().Position);
        }

        if (QuickTimeMovement.Instance().isActive)
        {
            QuickTimeMovement.Instance().ReceiveMousePos(Mouse.GetState().Position);
        }

        if(!pressHandled && Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            Board.Instance().OnClick(Mouse.GetState().Position);

            // do after, no time to explain
            if (QuickTimeMovement.Instance().isActive)
            {
                QuickTimeMovement.Instance().OnClick();
            }
            pressHandled = true;
        }
        else if (pressHandled && Mouse.GetState().LeftButton == ButtonState.Released) {
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

            spriteBatch.DrawString(oldenburg_40, "Enemies " + PlayerManager.Instance().ghostPlayers.Count, new Vector2(1500f, 100f), Color.White);
        }
        spriteBatch.End();

        base.Draw(gameTime);
    }

    public static void Main()
    {
        using (var game = new Game1())
            game.Run();
    }

    public static void OnGameOver(GhostMeeple winner)
    {
        Console.WriteLine("game over, "+winner.ghostPlayer.name+" won!");
    }

    protected override void OnExiting(object sender, EventArgs args)
    {
        PlayerManager.Instance().local.DisconnectClient();
        base.OnExiting(sender, args);
    }
}
