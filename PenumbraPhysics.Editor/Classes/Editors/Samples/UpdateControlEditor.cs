using System;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PenumbraPhysics.Editor.Classes.Basic;
using System.Collections.Generic;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;

namespace PenumbraPhysics.Editor.Classes
{
    public class WelcomeUpdateControlSampleEditor : GFXPhysicsService
    {
        // Store reference to lighting system
        public PenumbraComponent Penumbra;

        // Penumbra light
        private Light _light;

        // The physics body
        Body tBody;
        Texture2D tBodyTexture;
        Vector2 tBodyOrigin;
        List<Hull> tBodyHull = new List<Hull>();
        float tBodyScale = 1f;
        
        Vector2 MGLogoDirection = new Vector2(0.5f, 0.5f);
        bool HitBorder = false;
        float DirectionCooldown = 2f, DirectionCooldownMax = 2f;

        public WelcomeUpdateControlSampleEditor(IGraphicsDeviceService graphics)
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
                Position = new Vector2(-250, -250),
                Color = Color.White,
                Scale = new Vector2(1700),
                Radius = 30f,
                ShadowType = ShadowType.Illuminated
            };
            Penumbra.Lights.Add(_light);

            // Loading the texture of the physics object
            tBodyTexture = Content.Load<Texture2D>(@"Samples/MGLogo");

            // Creating the physics object
            tBody = CreateComplexBody(_World, tBodyTexture, tBodyScale, out tBodyOrigin);
            tBody.SleepingAllowed = false;
            tBody.Position = ConvertUnits.ToSimUnits(new Vector2(tBodyTexture.Width / 2, tBodyTexture.Height / 2));
            tBody.BodyType = BodyType.Dynamic;
            tBody.AngularDamping = 2f;
            tBody.Restitution = 1f;
            tBody.FixedRotation = true;
            tBody.OnCollision += TBody_OnCollision;
            tBody.UserData = new PhysicsBodyFlags()
            {
                HullList = new List<Hull>(),
                StartPosition = tBody.Position,
                StartRotation = tBody.Rotation
            };

            CreateShadowHulls(Penumbra, tBody);
            CreatePhysicalBorders();
        }

        private bool TBody_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (!HitBorder)
            {
                HitBorder = true;
                if ((Body)fixtureB.Body != null && ((Body)fixtureB.Body).UserData != null)
                {
                    if (((Body)fixtureB.Body).UserData.ToString() == "D" ||
                        ((Body)fixtureB.Body).UserData.ToString() == "U") MGLogoDirection.Y *= -1;
                    else if (((Body)fixtureB.Body).UserData.ToString() == "L" ||
                        ((Body)fixtureB.Body).UserData.ToString() == "R") MGLogoDirection.X *= -1;
                }
            }
            return true;
        }

        public void Update(GameTime gameTime, Vector2 mousePosition)
        {
            if (HitBorder)
            {
                if (DirectionCooldown > 0) DirectionCooldown--;
                else
                {
                    HitBorder = false;
                    DirectionCooldown = DirectionCooldownMax;
                }
            }

            tBody.Position += new Vector2(
                ConvertUnits.ToSimUnits(MGLogoDirection).X * 2,
                ConvertUnits.ToSimUnits(MGLogoDirection).Y * 2);

            UpdateShadowHulls(tBody);
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
                            Color.White, tBody.Rotation, tBodyOrigin, tBodyScale, SpriteEffects.None, 0);
                spriteBatch.End();
            }

            Penumbra.Draw();

            DrawPhysicsDebugView();
            DrawDisplay();
        }
    }
}