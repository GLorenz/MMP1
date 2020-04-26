using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

public class Game1 : Game
{
    public int windowWidth;
    public int windowHeight;
    GraphicsDeviceManager graphics;
    SpriteBatch spriteBatch;

    Texture2D redTex, greenTex;
    Rectangle redRect, greenRect;
    string redUID = "red";
    string greenUID = "green";

    List<Player> players;

    public Game1()
    {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        players = new List<Player>();
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
        redTex = Content.Load<Texture2D>("red");
        greenTex = Content.Load<Texture2D>("green");

        redRect = new Rectangle(100, 100, redTex.Width, redTex.Height);
        greenRect = new Rectangle(100, 200, greenTex.Width, greenTex.Height);
        CreatePlayers();
    }

    protected void CreatePlayers()
    {
        Player pRalph = new Player("Ralph");
        //Player pLucie = new Player("Lucie");
        players.Add(pRalph);
        //players.Add(pLucie);

        Meeple meepleRed = new Meeple(pRalph, redUID, redRect, redTex);
        //Meeple meepleGreen = new Meeple(pLucie, greenUID, greenRect, greenTex);

        Board.Instance().AddElement(meepleRed);
        //Board.Instance().AddElement(meepleGreen);
    }
    
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
        /*foreach(Player p in players)
        {
            p.client.socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
            p.client.socket.Close();
        }*/
    }

    bool pressHandled;
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        if(Mouse.GetState().LeftButton == ButtonState.Pressed && !pressHandled)
        {
            MoveCommand mcmd = new MoveCommand((MovingBoardElement)Board.Instance().FindByUID(redUID), Mouse.GetState().Position);
            //Input input = new Input("Move", redUID, Mouse.GetState().Position.X+","+Mouse.GetState().Position.Y, false);
            Player.local.HandleInput(mcmd, true);
            pressHandled = true;
        }
        else if (Mouse.GetState().LeftButton == ButtonState.Released && pressHandled) { pressHandled = false; }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        spriteBatch.Begin();

        foreach(BoardElement element in Board.Instance().boardElements)
        {
            spriteBatch.Draw(element.texture, element.Position, Color.White);
        }
        spriteBatch.End();

        base.Draw(gameTime);
    }

    public static void Main()
    {
        using (var game = new Game1())
            game.Run();
    }
}
