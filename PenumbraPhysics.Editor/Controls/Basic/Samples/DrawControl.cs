using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace PenumbraPhysics.Editor.Controls
{
    public class DrawControlSAMPLE : GraphicsDeviceControl
    {
        ContentManager Content { get; set; }
        SpriteBatch spriteBatch { get; set; }

        SpriteFont Font { get; set; }
        const string HelloWorld = "This is a simple draw control without an update cycle.\n It's just for your drawing needs :)";

        protected override void Initialize()
        {
            Content = new ContentManager(Services, "Content");
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Font = Content.Load<SpriteFont>(@"Font");
        }

        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.DrawString(Font, HelloWorld, new Vector2(
                (GraphicsDevice.Viewport.Width / 2) - (Font.MeasureString(HelloWorld).X / 2), 
                (GraphicsDevice.Viewport.Height / 2) - (Font.MeasureString(HelloWorld).Y / 2)), Color.White);

            spriteBatch.End();

            Invalidate(); // You need this to reflect drawing changes in the control.
        }
    }
}
