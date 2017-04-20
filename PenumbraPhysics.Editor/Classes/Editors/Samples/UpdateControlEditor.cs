using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PenumbraPhysics.Editor.Classes
{
    public class UpdateControlSampleEditor
    {
        private GameServiceContainer services;
        private GraphicsDevice graphics;
        private ContentManager Content;
        private SpriteBatch spriteBatch;

        private SpriteFont Font;

        private Texture2D _xor;
        private Vector2 _xorPosition;
        private Vector2 _xorDirection;

        private Vector2 GetMousePosition { get; set; }

        private TimeSpan elapsedTime = TimeSpan.Zero;
        private int frameCounter, frameRate;
        private System.Globalization.NumberFormatInfo format;

        public UpdateControlSampleEditor(IGraphicsDeviceService graphics)
        {
            services = new GameServiceContainer();
            services.AddService(typeof(IGraphicsDeviceService), graphics);

            this.graphics = graphics.GraphicsDevice;

            Content = new ContentManager(services, "Content");
            spriteBatch = new SpriteBatch(this.graphics);

            format = new System.Globalization.NumberFormatInfo();
            format.CurrencyDecimalSeparator = ".";
        }

        public void Initialize()
        {
            _xor = BuildXorTexture(graphics, 6);
            _xorDirection = new Vector2(.5f, .5f);

            Font = Content.Load<SpriteFont>(@"Font");
        }

        public void Update(GameTime gameTime, Vector2 mousePosition)
        {
            float limitX = graphics.Viewport.Width - _xor.Width;
            float limitY = graphics.Viewport.Height - _xor.Height;

            if (_xorPosition.X >= limitX && _xorDirection.X > 0)
                _xorDirection.X *= -1;
            if (_xorPosition.X <= 0 && _xorDirection.X < 0)
                _xorDirection.X *= -1;
            if (_xorPosition.Y >= limitY && _xorDirection.Y > 0)
                _xorDirection.Y *= -1;
            if (_xorPosition.Y <= 0 && _xorDirection.Y < 0)
                _xorDirection.Y *= -1;

            _xorPosition += _xorDirection;

            GetMousePosition = mousePosition;

            //Für den FPS-Counter (steht am Ende)
            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime <= TimeSpan.FromSeconds(1)) return;
            elapsedTime -= TimeSpan.FromSeconds(1);
            frameRate = frameCounter;
            frameCounter = 0;
            // Nachfolgend darf kein Code mehr geschrieben werden!
        }

        public void Draw(GameTime gameTime)
        {
            frameCounter++;

            graphics.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(_xor, _xorPosition, Color.White);

            string fps = string.Format(format, "{0} fps", frameRate);
            string drawString = $"{fps}\n{GetMousePosition}";
            spriteBatch.DrawString(Font, drawString, new Vector2(0, 0), Color.White);

            spriteBatch.End();
        }

        private static Texture2D BuildXorTexture(GraphicsDevice device, int bits)
        {
            Texture2D tex = new Texture2D(device, 1 << bits, 1 << bits);
            Color[] data = new Color[tex.Width * tex.Height];

            for (int y = 0; y < tex.Height; y++)
            {
                for (int x = 0; x < tex.Width; x++)
                {
                    float lum = ((x << (8 - bits)) ^ (y << (8 - bits))) / 255f;
                    data[y * tex.Width + x] = new Color(lum, lum, lum);
                }
            }

            tex.SetData(data);
            return tex;
        }
    }
}