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
    public class WelcomeUpdateSampleEditor : GFXPhysicsService
    {
        // Penumbra light
        private Light _light;

        // The physics body
        Body tBody;
        Texture2D tBodyTexture;
        Vector2 tBodyOrigin;
        List<Hull> tBodyHull = new List<Hull>();
        float tBodyScale = 1f;

        Texture2D MGLogo;
        Vector2 PenumbraPhysicsLogoDirection = new Vector2(0.5f, 0.5f);
        bool HitBorder = false;
        float DirectionCooldown = 2f, DirectionCooldownMax = 2f;

        public WelcomeUpdateSampleEditor(IGraphicsDeviceService graphics)
        {
            // Initialize GFX-System
            InitializeGFX(graphics);

            // Initialize Physics-System
            InitializePhysics(graphics.GraphicsDevice, Content);
        }

        public void Initialize()
        {
            Penumbra.AmbientColor = new Color(new Vector3(0.8f));

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
            tBodyTexture = Content.Load<Texture2D>(@"Samples/PenumbraPhysicsEditorLogo");
            MGLogo = Content.Load<Texture2D>(@"Samples/MGHorizontalLogo");

            // Creating the physics object
            tBody = CreateComplexBody(_World, tBodyTexture, tBodyScale, out tBodyOrigin);
            tBody.SleepingAllowed = false;
            tBody.Position = ConvertUnits.ToSimUnits(new Vector2((tBodyTexture.Width / 2) + 25, (tBodyTexture.Height / 2) + 25));
            tBody.BodyType = BodyType.Dynamic;
            tBody.AngularDamping = 2f;
            tBody.Restitution = 1f;
            tBody.FixedRotation = true;
            tBody.OnCollision += TBody_OnCollision;
            tBody.UserData = new BodyFlags()
            {
                HullList = new List<Hull>(),
                ShadowHullScale = -2f,
                StartPosition = tBody.Position,
                StartRotation = tBody.Rotation
            };

            CreateShadowHulls(tBody);
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
                        ((Body)fixtureB.Body).UserData.ToString() == "U") PenumbraPhysicsLogoDirection.Y *= -1;
                    else if (((Body)fixtureB.Body).UserData.ToString() == "L" ||
                        ((Body)fixtureB.Body).UserData.ToString() == "R") PenumbraPhysicsLogoDirection.X *= -1;
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
                ConvertUnits.ToSimUnits(PenumbraPhysicsLogoDirection).X * 2,
                ConvertUnits.ToSimUnits(PenumbraPhysicsLogoDirection).Y * 2);

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

            graphics.Clear(BackgroundColor);

            if (tBody != null)
            {
                spriteBatch.Begin();

                // Draw the texture of the MGLogo
                spriteBatch.Draw(MGLogo, new Vector2(-7 + ViewportWidth - MGLogo.Width / 2, 7 + MGLogo.Height / 2), null,
                            Color.White, 0, new Vector2(MGLogo.Width / 2, MGLogo.Height / 2), 1f, SpriteEffects.None, 0);

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