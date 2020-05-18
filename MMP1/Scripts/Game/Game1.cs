// Author: Lorenz Gonsa
// Company: FHS-MMT
// Project: MultiMediaProject 1

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

public class Game1 : Game
{
    public enum NetworkType { Online, Offline, Local }
    public static NetworkType networkType = NetworkType.Online;
    
    public static int meepleCount = 2;

    public static int windowWidth { get; private set; }
    public static int windowHeight { get; private set; }
    
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;

    //private SpriteFont oldenburg_30, oldenburg_60, oldenburg_20, josefin_20;

    private bool pressHandled;

    // c# says no, when using class without generics, and a simple downcast isn't possible :(
    private GenericBoardElementHolder<BoardElement> elementsHolder;

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
        Window.IsBorderless = true;
        Window.Position = new Point(50,50);
        graphics.PreferredBackBufferWidth = windowWidth;
        graphics.PreferredBackBufferHeight = windowHeight;
        IsMouseVisible = true;
        //graphics.IsFullScreen = true;
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

        SpriteFont oldenburg_8 = Content.Load<SpriteFont>("fonts/oldenburg_8");
        SpriteFont oldenburg_20 = Content.Load<SpriteFont>("fonts/oldenburg_20");
        SpriteFont oldenburg_30 = Content.Load<SpriteFont>("fonts/oldenburg_30");
        SpriteFont oldenburg_60 = Content.Load<SpriteFont>("fonts/oldenburg_60");
        SpriteFont josefin_20 = Content.Load<SpriteFont>("fonts/josefin_20");

        QuestionManager.Instance().SetQuestionFont(oldenburg_60);
        QuestionManager.Instance().SetAnswerFont(oldenburg_30);

        FontResources.oldenburg_8 = oldenburg_8;
        FontResources.oldenburg_20 = oldenburg_20;
        FontResources.oldenburg_30 = oldenburg_30;
        FontResources.oldenburg_60 = oldenburg_60;
        FontResources.josefin_20 = josefin_20;

        new Setupper(windowWidth, windowHeight).Setup();
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
