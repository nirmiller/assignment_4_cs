using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace group_14_assignment4;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    // fields for meteor  
    private Meteor meteor;
    private Meteor comet;
    
    private Vector2 meteorStartPosition;
    private Vector2 cometStartPosition;
    
    // fields for planet  
    private Texture2D _planet; 
    private Vector2 _planetPosition;
    
    // fields for rocket  
    private Texture2D _rocket1Texture;
    private Texture2D _1exhaustTexture;
    private Rocket _smallerRocket;
    
    private Texture2D _rocket2Texture;
    private Texture2D _2exhaustTexture;
    private Rocket _LargerRocket2;
    
    // fields for star
    private Texture2D _starTexture;
    private Texture2D _sparkleTexture;

    private List<Star> _stars;

    // screen size info
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
        // initialization for planet position 
        _planetPosition = new Vector2(-250, 500);
        
        // initialization for stars 
        _stars = new List<Star>();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        // loading content for meteor
        meteorStartPosition = new Vector2(1000, 50);
        cometStartPosition = new Vector2(850, 150);
        
        meteor = new Meteor(Content.Load<Texture2D>("imgs/meteor_body"), 
            Content.Load<Texture2D>("imgs/meteor_tail")
            ,2f, meteorStartPosition);
        comet = new Meteor(Content.Load<Texture2D>("imgs/comet_body"), 
            Content.Load<Texture2D>("imgs/comet_tail")
            ,2f, cometStartPosition);
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // loading content for planet 
        _planet = Content.Load<Texture2D>("imgs/planet");
        
        // loading content for rocket
        _rocket1Texture = Content.Load<Texture2D>("imgs/rocket1");
        _rocket2Texture = Content.Load<Texture2D>("imgs/rocket2");
        
        // load content for exhust 
        _1exhaustTexture = Content.Load<Texture2D>("imgs/fire1");
        _2exhaustTexture = Content.Load<Texture2D>("imgs/fire2");

        _smallerRocket = new Rocket(_rocket1Texture, 
            _1exhaustTexture, 
            new Vector2(600, 1200),
            new Vector2(0, 150), 
            1.0f, 
            0.99f, 
            1.0f, 
            0.09f,
            7.0f, 
            20.0f,
            -30.0f);
        
        //rocket2
        _LargerRocket2 = new Rocket(_rocket2Texture,
            _2exhaustTexture,
            new Vector2(50, 1000),
            new Vector2(0, 100),
            1.0f,
            0.99f,
            1.0f,
            0.09f,
            25.0f,
            20.0f,
            -70.0f);


        // TODO: use this.Content to load your game content here
        // loading content for stars 
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
        
        // update for meteor
        meteor.Animate(gameTime, meteorStartPosition, new Vector2(0, 300), 5);
        comet.Animate(gameTime, cometStartPosition, new Vector2(0, 500), 5);
        
        // update for rocket 
        _smallerRocket.Update(gameTime);
        _LargerRocket2.Update(gameTime);
        
        
        // update for stars
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
        
        // drawing for stars 
        foreach (var star in _stars)
        {
            _spriteBatch.Begin(transformMatrix: star.WorldMatrix);
            star.Draw(_spriteBatch);
            _spriteBatch.End();
        }
        
        _smallerRocket.Draw(_spriteBatch);

        
        // drawing for meteor 
        meteor.Draw(_spriteBatch);
        comet.Draw(_spriteBatch);
    
        
        //drawing planet 
        _spriteBatch.Begin();
        _spriteBatch.Draw(
            _planet,
            _planetPosition,
            sourceRectangle: null,
            Color.White,
            0.0f,
            Vector2.Zero,
            scale: 2.0f,
            SpriteEffects.None,
            layerDepth: 0.0f
        );
        _spriteBatch.End();
        
        // drawing rocket 
        _LargerRocket2.Draw(_spriteBatch);
        
       

        base.Draw(gameTime);
    }
}