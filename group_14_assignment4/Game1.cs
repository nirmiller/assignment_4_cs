using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_14_assignment4;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Meteor meteor;
    private Meteor comet;
    
    private Vector2 meteorStartPosition;
    private Vector2 cometStartPosition;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);

        _graphics.PreferredBackBufferHeight = 800;
        _graphics.PreferredBackBufferWidth = 800;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        
        meteorStartPosition = new Vector2(100, 100);
        cometStartPosition = new Vector2(0, 100);
        
        meteor = new Meteor(Content.Load<Texture2D>("imgs/meteor_body"), 
            Content.Load<Texture2D>("imgs/meteor_tail")
            ,2f, meteorStartPosition);
        comet = new Meteor(Content.Load<Texture2D>("imgs/comet_body"), 
            Content.Load<Texture2D>("imgs/comet_tail")
            ,2f, cometStartPosition);
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        
        meteor.Animate(gameTime, meteorStartPosition, new Vector2(800, 800), 5);
        comet.Animate(gameTime, cometStartPosition, new Vector2(500, 500), 5);
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.MidnightBlue);

        // TODO: Add your drawing code here
        meteor.Draw(_spriteBatch);
        comet.Draw(_spriteBatch);

        base.Draw(gameTime);
    }
}