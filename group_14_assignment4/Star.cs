using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace group_14_assignment4
{
    public class Star
    {
        private Texture2D _starTexture;
        private Texture2D _sparkleTexture;

        private Vector2 _position;
        private float _rotation;
        private float _scale;

        private Vector2 _velocity;
        private float _rotationSpeed;

        private Matrix _worldMatrix;

        // Sparkle fields (child)
        private Vector2 _sparkleOffset;
        private float _sparkleTimer;
        private float _sparkleSpeed;

        private int _screenWidth;
        private int _screenHeight;

        private static Random _random = new Random();

        public Star(Texture2D starTexture, Texture2D sparkleTexture,
                    int screenWidth, int screenHeight)
        {
            _starTexture = starTexture;
            _sparkleTexture = sparkleTexture;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;

            Initialize();
        }

        private void Initialize()
        {
            _position = new Vector2(
                _random.Next(-_screenWidth, _screenWidth),
                _random.Next(-100, _screenHeight + 100)
            );

            _rotation = MathHelper.ToRadians(_random.Next(0, 360));
            _scale = 0.015f + (float)_random.NextDouble() * 0.03f;

            float speed = 30f + (float)_random.NextDouble() * 20f;
            _velocity = new Vector2(speed, -speed / 6f);

            _rotationSpeed = MathHelper.ToRadians(
                _random.Next(-30, 30)
            );

            // Sparkle local offset
            _sparkleOffset = new Vector2(
                _random.Next(-20, 20),
                _random.Next(-20, 20)
            );

            _sparkleSpeed = 2f + (float)_random.NextDouble() * 3f;
            _sparkleTimer = (float)_random.NextDouble() * MathHelper.TwoPi;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _position += _velocity * dt;
            _rotation += _rotationSpeed * dt;

            _sparkleTimer += dt * _sparkleSpeed;

            if (_position.X > _screenWidth + 20 || _position.Y < -50)
            {
                RespawnFromLeft();
            }

            _worldMatrix =
                Matrix.CreateScale(_scale) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateTranslation(new Vector3(_position, 0f));
        }

        private void RespawnFromLeft()
        {
            _position.X = -20;
            _position.Y = _random.Next(-100, _screenHeight + 200);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 starOrigin = new Vector2(
                _starTexture.Width / 2f,
                _starTexture.Height / 2f
            );

            spriteBatch.Draw(
                _starTexture,
                Vector2.Zero,
                null,
                Color.White,
                0f,
                starOrigin,
                1f,
                SpriteEffects.None,
                0f
            );

            // ----- CHILD SPARKLE DRAW -----

            float alpha = (float)(0.5f + 0.5f * Math.Sin(_sparkleTimer));

            // Color shifting (fake hue shift)
            float r = 0.5f + 0.5f * (float)Math.Sin(_sparkleTimer);
            float g = 0.5f + 0.5f * (float)Math.Sin(_sparkleTimer + 2f);
            float b = 0.5f + 0.5f * (float)Math.Sin(_sparkleTimer + 4f);

            Color sparkleColor = new Color(r, g, b) * alpha;

            Vector2 sparkleOrigin = new Vector2(
                _sparkleTexture.Width / 2f,
                _sparkleTexture.Height / 2f
            );

            spriteBatch.Draw(
                _sparkleTexture,
                _sparkleOffset,   // local position (child transform)
                null,
                sparkleColor,
                0f,
                sparkleOrigin,
                10.0f,
                SpriteEffects.None,
                0f
            );
        }

        public Matrix WorldMatrix => _worldMatrix;
    }
}