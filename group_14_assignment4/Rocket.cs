using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace group_14_assignment4;

public class Rocket
{
    // fields 
    // textures 
    private Texture2D _rocketBody;
    
    // movement 
    private Vector2 _startRocketPosition;
    private Vector2 _initialRocketPosition;
    private Vector2 _rocketSpeed;
    
    // scaling
    private float _maxRocketScale;      
    private float _minRocketScale;  
    private float _scaleSpeed; 
    private float _currentRocketScale;
    private bool _isScalingDown = true;

    // exhaust
    private Exhaust _exhaust;  
    private float _exhaustShakeAmount;
    private float _exhaustShakeSpeed;
    private float _exhaustVerticalOffset;
    
    // transformation
    private Matrix _rocketTransformation = Matrix.Identity;
    
    // constructor 
    public Rocket(Texture2D rocketBody, 
        Texture2D exhaustTexture, 
        Vector2 startRocketPosition, 
        Vector2 rocketSpeed,
        float maxRocketScale, 
        float minRocketScale, 
        float initialRocketScale, 
        float scaleSpeed, 
        float exhaustShakeAmount, 
        float exhaustShakeSpeed,
        float exhaustVerticalOffset)
    {
        _rocketBody = rocketBody;
        _startRocketPosition = startRocketPosition;
        _initialRocketPosition = startRocketPosition;
        _rocketSpeed = rocketSpeed;

        _maxRocketScale = maxRocketScale;
        _minRocketScale = minRocketScale;
        _currentRocketScale = initialRocketScale;
        _scaleSpeed = scaleSpeed;
        
        _exhaustVerticalOffset = exhaustVerticalOffset;
    
        _exhaust = new Exhaust(exhaustTexture, exhaustShakeAmount, exhaustShakeSpeed, _exhaustVerticalOffset);
    }
    
    // overall movement 
    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // move rocket
        _startRocketPosition.Y -= _rocketSpeed.Y * dt;
        
        if (_isScalingDown)
        {
            _currentRocketScale -= _scaleSpeed * dt;

            // if it hits the min scale make _isScalingDown false
            if (_currentRocketScale <= _minRocketScale)
            {
                _currentRocketScale = _minRocketScale;
                _isScalingDown = false;
            }
        }
        else
        {
            _currentRocketScale += _scaleSpeed * dt;
            
            // changes _isScalingDown to true when it hits max
            if (_currentRocketScale >= _maxRocketScale)
            {
                _currentRocketScale = _maxRocketScale;
                _isScalingDown = true;
            }
        }
        
        // creating rocket transform matrix 
        _rocketTransformation =
            Matrix.CreateScale(_currentRocketScale) *
            Matrix.CreateTranslation(new Vector3(_startRocketPosition, 1));
        
        // exhaust
        Vector2 rocketSize = new Vector2(_rocketBody.Width, _rocketBody.Height);
        _exhaust.SetOffSetFromRocket(rocketSize);
        
        // resetting
        if (IsAboveScreen())
        {
            Reset();
        }
        
        _exhaust.Update(gameTime);
}

    public void Reset()
    {
        _startRocketPosition = _initialRocketPosition;
        _currentRocketScale = _maxRocketScale;
        _isScalingDown = true;
    }

    public bool IsAboveScreen()
    {
        return _startRocketPosition.Y + (_rocketBody.Height * _currentRocketScale) < 0;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // drawing exhaust (drawing this first so it will appear on bottom)
        _exhaust.Draw(spriteBatch, _rocketTransformation);
        
        // drawing rocket
        spriteBatch.Begin(transformMatrix: _rocketTransformation);
        spriteBatch.Draw(_rocketBody, Vector2.Zero, Color.White);
        spriteBatch.End();
    }
}

public class Exhaust
{
    // texture 
    private Texture2D _exhaustTexture;

    // animation settings 
    private float _shakeAmount;     // how far it moves vertically
    private float _shakeSpeed;      // how fast is oscillates
    
    private float _time;        // tracks sin wave phase 
    private Vector2 _offSetFromRocket;
    private Matrix _exhaustTransform = Matrix.Identity;
    private float _verticalOffset;
    

    public Exhaust(Texture2D exhaustTexture, float shakeAmount, float shakeSpeed, float verticalOffset)
    {
        _exhaustTexture = exhaustTexture;
        _shakeAmount = shakeAmount;
        _shakeSpeed = shakeSpeed;
        _offSetFromRocket = Vector2.Zero;
        _verticalOffset = verticalOffset; 
    }
    
    public void SetOffSetFromRocket(Vector2 rocketSize)
    {
        _offSetFromRocket = new Vector2((rocketSize.X / 2f) , rocketSize.Y + _verticalOffset);
    }

    public void Update(GameTime gameTime)
    {
        _time += (float)gameTime.ElapsedGameTime.TotalSeconds;

        // sine wave shake
        float yOffset = (float)Math.Sin(_time * _shakeSpeed) * _shakeAmount;

        _exhaustTransform =
            Matrix.CreateTranslation(new Vector3(_offSetFromRocket.X - (_exhaustTexture.Width / 2f), 
                _offSetFromRocket.Y + yOffset, 0));
    }

    public void Draw(SpriteBatch spriteBatch, Matrix parentTransform)
    {
        Matrix world =
            _exhaustTransform *
            parentTransform;

        spriteBatch.Begin(transformMatrix: world);
        spriteBatch.Draw(_exhaustTexture, Vector2.Zero, Color.White);
        spriteBatch.End();
    }
}