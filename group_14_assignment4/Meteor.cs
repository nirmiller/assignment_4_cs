using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_14_assignment4;

public class Meteor
{
    private Texture2D _bodyTexture;
    private Texture2D _tailTexture;

    private Vector2 _bodyPosition;
    private Vector2 _tailPosition;
    private float _bodyRotation;
    private float _bodySpeed;
    private float _bodyWiggle;
    private float _size;
    private float _tailWiggle;

    public Meteor(Texture2D bodyTexture, Texture2D tailTexture)
    {
        _bodyTexture = bodyTexture;
        _tailTexture = tailTexture;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        Vector2 parentOrigin = new Vector2(_bodyTexture.Width / 2f, _bodyTexture.Height / 2f);
        Vector2 childOrigin  = new Vector2(_tailTexture.Width / 2f, _tailTexture.Height / 2f);

        
        // Moves parent spot around the screen
        Matrix M_parent =
            Matrix.CreateTranslation(-parentOrigin.X, -parentOrigin.Y, 0f) * 
            Matrix.CreateTranslation(_bodyPosition.X, _bodyPosition.Y, 0f);

        spriteBatch.Begin(transformMatrix: M_parent*Matrix.CreateRotationZ(_bodyRotation));
        spriteBatch.Draw(_bodyTexture, Vector2.Zero, Color.White); 
        spriteBatch.End();

        
        // Hierarchy from parent node, and wiggles fire side-to-side locally 
        Matrix M_childLocal =
            Matrix.CreateTranslation(-childOrigin.X, -childOrigin.Y, 0f) *
            Matrix.CreateRotationZ(_tailWiggle) *
            Matrix.CreateTranslation(-70f, 0f, 0f);

        
        // Composes the child and parent transformations 
        Matrix M_childWorld = M_childLocal * M_parent;

        spriteBatch.Begin(transformMatrix: M_childWorld);
        spriteBatch.Draw(_tailTexture, Vector2.Zero, Color.White);
        spriteBatch.End();
    }
}