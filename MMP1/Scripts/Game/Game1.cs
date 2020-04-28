using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class Game1 : Game
{
    public enum NetworkType { Online, Offline }
    public static NetworkType networkType = NetworkType.Online;

    public int windowWidth;
    public int windowHeight;
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;
    SpriteFont oldenburgFont;

    readonly string[] texNames = new string[] { "red", "green" };
    Meeple[] meeps;

    bool pressHandled;

    // c# says no, when using class without generics, and a simple downcast isn't possible :(
    GenericBoardElementHolder<BoardElement> boardElements;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        boardElements = Board.Instance();

        meeps = new Meeple[1];
    }
    protected override void Initialize()
    {
        // setting up display size here, since graphics aren't initialized in constructor
        windowWidth = GraphicsDevice.DisplayMode.Width;
        windowHeight = GraphicsDevice.DisplayMode.Height;
        Window.IsBorderless = false;
        Window.Position = new Point(50,50);
        graphics.PreferredBackBufferWidth = windowWidth - 100;
        graphics.PreferredBackBufferHeight = windowHeight - 100;
        IsMouseVisible = true;
        graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        TextureResources.contentManager = Content;

        TextureResources.AddAll(texNames);

        oldenburgFont = Content.Load<SpriteFont>("fonts/arial_16_bold");

        CreatePlayer();
        CreateMeeples();
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
        for (int i = 0; i < meeps.Length; i++)
        {
            Texture2D redTex = TextureResources.Get(texNames[new Random().Next(texNames.Length)]);
            meeps[i] = new Meeple(PlayerManager.Instance().local, new Rectangle(100 * i, 100, redTex.Width, redTex.Height), redTex);
            boardElements.AddElement(meeps[i]);

            CreateGhostMeepleCommand meepCmd = new CreateGhostMeepleCommand(meeps[i]);
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
            MoveCommand mcmd = new MoveCommand((MovingBoardElement)Board.Instance().FindByUID(meeps[0].UID), Mouse.GetState().Position);
            PlayerManager.Instance().local.HandleInput(mcmd, true);
            pressHandled = true;
        }
        else if (Mouse.GetState().LeftButton == ButtonState.Released && pressHandled) { pressHandled = false; }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();

        foreach(BoardElement element in boardElements.boardElements)
        {
            spriteBatch.Draw(element.texture, element.Position, Color.White);
            spriteBatch.DrawString(oldenburgFont, "Enemys " + PlayerManager.Instance().ghostPlayers.Count, new Vector2(1500f, 100f), Color.White);
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
