using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace PenumbraPhysics.Editor.Classes.Basic
{
    public interface IGFXInterface
    {
        GameServiceContainer services { get; set; }
        GraphicsDevice graphics { get; set; }
        SpriteBatch spriteBatch { get; set; }
        ContentManager Content { get; set; }

        // Display
        SpriteFont Font { get; set; }
        Vector2 GetMousePosition { get; set; }
        int FrameCounter { get; set; }
        TimeSpan ElapsedTime { get; set; }
        int FrameRate { get; set; }
        System.Globalization.NumberFormatInfo Format { get; set; }

        void InitializeGFX(IGraphicsDeviceService graphics);
        void UpdateFrameCounter();
        void UpdateDisplay(GameTime gameTime, Vector2 mousePosition);
        void DrawDisplay();
    }

    public interface IPhysicsInterface
    {
        World _World { get; set; }
        DebugViewXNA PhysicsDebugView { get; set; }
        FixedMouseJoint FixedMouseJoint { get; set; }

        float MeterInPixels { get; set; }

        int ViewportWidth { get; set; }
        int ViewportHeight { get; set; }

        void InitializePhysics(GraphicsDevice graphics, ContentManager Content);

        void UpdatePhysics(GameTime gameTime);
        void UpdatePhysicsManipulation(bool leftMouseButtonPressed, Vector2 mousePosition);

        void DrawPhysicsDebugView();
    }
}
