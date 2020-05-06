using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class Game1 : Game
{
    public enum NetworkType { Online, Offline, Local }
    public static NetworkType networkType = NetworkType.Offline;

    public static int playerCount = 4;
    public static int meepleCount = 2;

    public int windowWidth;
    public int windowHeight;
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
        CreatePlayer();
        CreateMeeples();
    }

    protected void SetupBoard()
    {
        Texture2D background = TextureResources.Get("Background");
        float backgroundAspectRatio = background.Width / (float)background.Height;
        int backgroundWidth = windowWidth;
        int backgroundHeight = (int)(backgroundWidth / backgroundAspectRatio);
        int backgroundX = (windowWidth - backgroundWidth) / 2;
        int backgroundY = (windowHeight - backgroundHeight) / 2;
        FieldBoardElement backgroundBoardEl = new FieldBoardElement(new Rectangle(backgroundX, backgroundY, backgroundWidth, backgroundHeight), background, zPosition:-10);
        Board.Instance().AddElement(backgroundBoardEl);

        Texture2D boardTex = TextureResources.Get("Board");
        float boardAspectRatio = boardTex.Width / (float)boardTex.Height;
        int boardHeight = windowHeight;
        int boardWidth = (int)(boardHeight * boardAspectRatio);
        int boardX = (windowWidth - boardWidth) / 2;
        int boardY = (windowHeight - boardHeight) / 2;
        FieldBoardElement board = new FieldBoardElement(new Rectangle(boardX, boardY, boardWidth, boardHeight), boardTex, zPosition:-1);
        Board.Instance().AddElement(board);

        //todo: calculate level values out of board rect
        PyramidLevel level0 = new PyramidLevel(UnitConvert.ToAbsolute(new Point(270, 68)), UnitConvert.ToAbsolute(new Point(697, 874)), 7, true);
        PyramidLevel level1 = new PyramidLevel(UnitConvert.ToAbsolute(new Point(327, 170)), UnitConvert.ToAbsolute(new Point(641, 775)), 5, false, "green");
        PyramidLevel level2 = new PyramidLevel(UnitConvert.ToAbsolute(new Point(379, 270)), UnitConvert.ToAbsolute(new Point(586, 666)), 4, false);
        Board.Instance().AddElement(level0.elements.ToArray());
        Board.Instance().AddElement(level1.elements.ToArray());
        Board.Instance().AddElement(level2.elements.ToArray());
    }

    protected void CreatePlayer()
    {
        Player pRalph = new Player("Ralph");
        PlayerManager.Instance().SetLocal(pRalph);

        CreateGhostPlayerCommand ghostCmd = new CreateGhostPlayerCommand(pRalph);
        PlayerManager.Instance().local.OnlyShare(ghostCmd);
    }

    protected void CreateMeeples()
    {
        MeepleColor color = MeepleColorClaimer.Next();
        for (int i = 0; i < meepleCount; i++)
        {
            Meeple newMeep = new Meeple(PlayerManager.Instance().local, new Rectangle(100 * i, 100, 100, 100), color);
            elementsHolder.AddElement(newMeep);

            CreateGhostMeepleCommand meepCmd = new CreateGhostMeepleCommand(newMeep);
            PlayerManager.Instance().local.OnlyShare(meepCmd);
        }
    }
    
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }
    
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if(Mouse.GetState().LeftButton == ButtonState.Pressed && !pressHandled)
        {
            /*MoveCommand mcmd = new MoveCommand((MovingBoardElement)Board.Instance().FindByUID(PlayerManager.Instance().GetLocalMeeples()[0].UID), Mouse.GetState().Position);
            PlayerManager.Instance().local.HandleInput(mcmd, true);*/
            Console.WriteLine(Mouse.GetState().Position);
            Console.WriteLine(UnitConvert.ToScreenRelative(Mouse.GetState().Position));
            pressHandled = true;
        }
        else if (Mouse.GetState().LeftButton == ButtonState.Released && pressHandled) {
            Console.WriteLine(Mouse.GetState().Position);
            Console.WriteLine(UnitConvert.ToScreenRelative(Mouse.GetState().Position));
            pressHandled = false;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin(SpriteSortMode.BackToFront);

        for (int i = 0; i < elementsHolder.boardElements.Count; i++)
        {
            spriteBatch.Draw(elementsHolder.boardElements[i].texture, elementsHolder.boardElements[i].Position, Color.White);
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

    protected override void OnExiting(object sender, EventArgs args)
    {
        PlayerManager.Instance().local.DisconnectClient();
        base.OnExiting(sender, args);
    }
}
