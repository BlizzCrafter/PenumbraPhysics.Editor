using System;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PenumbraPhysics.Editor.Classes.Basic;

namespace PenumbraPhysics.Editor.Classes.Editors.Samples
{
    public class PenumbraPhysicsEditor : GFXPhysicsService
    {
        // Store reference to lighting system
        public PenumbraComponent Penumbra;

        // Penumbra light
        private Light _light;

        // The physics body
        Body tBody;
        Texture2D tBodyTexture;
        Vector2 tBodyOrigin;
        float tBodyScale = 0.5f;

        public PenumbraPhysicsEditor(IGraphicsDeviceService graphics)
        {
            // Initialize GFX-System
            InitializeGFX(graphics);

            // Initialize Physics-System
            InitializePhysics(graphics.GraphicsDevice, Content);

            // Initialize Lighting-System
            Penumbra = new PenumbraComponent(this.graphics, Content);
        }

        public void Initialize()
        {
            Penumbra.AmbientColor = new Color(new Vector3(0.7f));

            _light = new PointLight
            {
                Position = new Vector2(-250, 0),
                Color = Color.White,
                Scale = new Vector2(1300),
                ShadowType = ShadowType.Solid
            };
            Penumbra.Lights.Add(_light);

            // Loading the texture of the physics object
            tBodyTexture = Content.Load<Texture2D>(@"Samples/object");

            // Creating the physics object
            tBody = CreateComplexBody(_World, tBodyTexture, tBodyScale, out tBodyOrigin);
            tBody.SleepingAllowed = false;
            tBody.Position = ConvertUnits.ToSimUnits(new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2));
            tBody.BodyType = BodyType.Dynamic;
            tBody.AngularDamping = 2f;
            tBody.Restitution = 1f;
            tBody.UserData = new PhysicsBodyFlags()
            {
                HullList = new List<Hull>(),
                StartPosition = tBody.Position,
                StartRotation = tBody.Rotation
            };

            CreateShadowHulls(Penumbra, tBody);
        }

        public void Update(GameTime gameTime, Vector2 mousePosition, bool leftMouseButtonPressed)
        {
            // Animate light position
            _light.Position =
                new Vector2(400f, 240f) + // Offset origin
                new Vector2( // Position around origin
                    (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds),
                    (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds)) * 240f;

            UpdateShadowHulls(tBody);
            UpdatePhysicsManipulation(leftMouseButtonPressed, mousePosition);
            UpdatePhysics(gameTime);
            UpdateDisplay(gameTime, mousePosition);
        }

        public void Draw(GameTime gameTime)
        {
            UpdateFrameCounter();

            // Everything between penumbra.BeginDraw and penumbra.Draw will be
            // lit by the lighting system.

            Penumbra.BeginDraw();

            graphics.Clear(Color.CornflowerBlue);

            if (tBody != null)
            {
                spriteBatch.Begin();

                // Draw the texture of the physics body
                spriteBatch.Draw(tBodyTexture, ConvertUnits.ToDisplayUnits(tBody.Position), null,
                            Color.Tomato, tBody.Rotation, tBodyOrigin, tBodyScale, SpriteEffects.None, 0);
                spriteBatch.End();
            }

            Penumbra.Draw();

            DrawPhysicsDebugView();
            DrawDisplay();
        }
    }
}