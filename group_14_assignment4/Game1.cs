using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace group_14_assignment4;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Meteor meteor;
    private Meteor comet;
    
    private Vector2 meteorStartPosition;
    private Vector2 cometStartPosition;
    
    private Texture2D _starTexture;
    private Texture2D _sparkleTexture;

    private List<Star> _stars;

    private int _screenWidth;
    private int _screenHeight;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);

        _graphics.PreferredBackBufferHeight = 800;
        _graphics.PreferredBackBufferWidth = 800;
        _screenWidth = 800;
        _screenHeight = 800;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _stars = new List<Star>();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        
        meteorStartPosition = new Vector2(800, 200);
        cometStartPosition = new Vector2(800, 400);
        
        meteor = new Meteor(Content.Load<Texture2D>("imgs/meteor_body"), 
            Content.Load<Texture2D>("imgs/meteor_tail")
            ,2f, meteorStartPosition);
        comet = new Meteor(Content.Load<Texture2D>("imgs/comet_body"), 
            Content.Load<Texture2D>("imgs/comet_tail")
            ,2f, cometStartPosition);
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _starTexture = Content.Load<Texture2D>("imgs/star");
        _sparkleTexture = Content.Load<Texture2D>("imgs/sparkle");

        for (int i = 0; i < 60; i++)
        {
            _stars.Add(new Star(_starTexture, _sparkleTexture,
                _screenWidth, _screenHeight));
        }
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        
        
        meteor.Animate(gameTime, meteorStartPosition, new Vector2(0, 300), 5);
        comet.Animate(gameTime, cometStartPosition, new Vector2(0, 500), 5);
        
        foreach (var star in _stars)
        {
            star.Update(gameTime);
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // TODO: Add your drawing code here
        
        foreach (var star in _stars)
        {
            _spriteBatch.Begin(transformMatrix: star.WorldMatrix);
            star.Draw(_spriteBatch);
            _spriteBatch.End();
        }
        
        meteor.Draw(_spriteBatch);
        comet.Draw(_spriteBatch);
        
       

        base.Draw(gameTime);
    }
}