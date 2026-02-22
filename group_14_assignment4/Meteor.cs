using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_14_assignment4;

public class Meteor
{
    private Texture2D _bodyTexture;
    private Texture2D _tailTexture;

    private Vector2 _bodyPosition;
    private float _bodyRotation;
    private float _bodyWiggle;
    private float _size;
    private float _tailWiggle;
    private float _tailWiggleSpeed;
    private Vector2 _startPosition;
    private float _startSize;
    private float stepSize;
    private float _spin;
    private float _t;

    private Vector2 _dir;

    public Meteor(Texture2D bodyTexture, Texture2D tailTexture, float size, Vector2 bodyPosition)
    {
        _bodyTexture = bodyTexture;
        _tailTexture = tailTexture;
        _size = size;
        _startSize = size;
        _t = 0;
        _bodyPosition = bodyPosition;
        _startPosition = bodyPosition;
        stepSize = 0;
        _tailWiggleSpeed = 2f;
        _dir = Vector2.UnitX;
    }

    public void ResetAnimation()
    {
        _t = 0f;
        _bodyPosition = _startPosition;
        _size = _startSize;
        stepSize = 0f;
        _dir = Vector2.UnitX;
        _bodyRotation = 0f;
    }

    public void Animate(GameTime gameTime, Vector2 start, Vector2 end, float duration)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _t += dt;
        float totalFrames = duration * 60f;
        float step = 1f / totalFrames;
        
        stepSize += step;
        _tailWiggle = .1f * (float)System.Math.Sin(_tailWiggleSpeed * _t);
        _spin += dt * 3f;   
        Vector2 v = end - start;
        if (v.LengthSquared() > 0f)
            _dir = Vector2.Normalize(v);

        _bodyRotation = (float)System.Math.Atan2(_dir.Y, _dir.X);

        _bodyPosition.X = MathHelper.Lerp(start.X, end.X, stepSize);
        _bodyPosition.Y = MathHelper.Lerp(start.Y, end.Y, stepSize);
        
        if (MathF.Abs(_bodyPosition.X - _startPosition.X) >= 1200 || MathF.Abs(_bodyPosition.Y - _startPosition.Y) >= 1200)
        {
            ResetAnimation();
        }
    }
    

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 parentOrigin = new Vector2(_bodyTexture.Width / 2f, _bodyTexture.Height / 2f);

        Matrix M_parent =
            Matrix.CreateTranslation(-parentOrigin.X, -parentOrigin.Y, 0f) *
            Matrix.CreateScale(_size, _size, 1f) *
            Matrix.CreateRotationZ(_bodyRotation) *
            Matrix.CreateTranslation(_bodyPosition.X, _bodyPosition.Y, 0f);

        Vector2 tailBaseOrigin = new Vector2(_tailTexture.Width, _tailTexture.Height);
        Vector2 socketLocal = new Vector2((_bodyTexture.Width / 2f), (_bodyTexture.Height / 2f));
        float tailArtOffset = -MathHelper.Pi / 3;

        Matrix M_childLocal =
            Matrix.CreateTranslation(-tailBaseOrigin.X, -tailBaseOrigin.Y, 0f) *
            Matrix.CreateRotationZ(tailArtOffset + _tailWiggle) *
            Matrix.CreateTranslation(socketLocal.X, socketLocal.Y, 0f)*
            Matrix.CreateScale(_size/2, _size/2, 1f);

        Matrix M_childWorld = M_childLocal * M_parent;

        spriteBatch.Begin(transformMatrix: M_childWorld);
        spriteBatch.Draw(_tailTexture, Vector2.Zero, Color.White);
        spriteBatch.End();

        Matrix M_body =
            Matrix.CreateTranslation(-parentOrigin.X, -parentOrigin.Y, 0f) *
            Matrix.CreateScale(_size, _size, 1f) *
            Matrix.CreateRotationZ(_bodyRotation + _spin) *
            Matrix.CreateTranslation(_bodyPosition.X, _bodyPosition.Y, 0f);
            

        spriteBatch.Begin(transformMatrix: M_body);
        spriteBatch.Draw(_bodyTexture, Vector2.Zero, Color.White);
        spriteBatch.End();
    }
}