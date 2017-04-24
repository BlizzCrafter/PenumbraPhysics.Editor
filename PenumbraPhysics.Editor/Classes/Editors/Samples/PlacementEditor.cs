using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using PenumbraPhysics.Editor.Classes.Basic;
using System.Collections.Generic;

namespace PenumbraPhysics.Editor.Classes.Editors.Samples
{
    public class PlacementEditor : GFXPhysicsService
    {
        private List<Light> LightList;
        private List<Body> LightPivots;
        private Body CurrentSelectedPivot;

        private Texture2D CenterTexture, MoveCamTexture;

        public void CreateLight()
        {
            if (LightList != null && LightPivots != null)
            {
                Vector2 position = new Vector2(ViewportWidth / 2, ViewportHeight / 2);

                //Create a light
                Light light = new PointLight
                {
                    Position = position,
                    Color = Color.White,
                    Scale = new Vector2(300),
                    ShadowType = ShadowType.Solid
                };
                LightList.Add(light);
                Penumbra.Lights.Add(light);
                
                //Create a pivot for the light
                Body bPivot = BodyFactory.CreateCircle(_World, ConvertUnits.ToSimUnits(5f), 1f);
                bPivot.Position = ConvertUnits.ToSimUnits(position);
                bPivot.BodyType = BodyType.Dynamic;
                bPivot.IsSensor = true;
                bPivot.FixedRotation = true;
                bPivot.LinearDamping = 999f;
                bPivot.Friction = 999f;
                bPivot.CollidesWith = Category.None;
                bPivot.CollisionCategories = Category.None;
                bPivot.UserData = new PivotBodyFlags()
                {
                    ConnectedObject = new ConnectedObject(light, position)
                };
                CurrentSelectedPivot = bPivot;
                LightPivots.Add(bPivot);
            }
        }

        public PlacementEditor(IGraphicsDeviceService graphics)
        {
            // Initialize GFX-System
            InitializeGFX(graphics);

            // Initialize Physics-System
            InitializePhysics(graphics.GraphicsDevice, Content);

            LightList = new List<Light>();
            LightPivots = new List<Body>();
        }

        public void Initialize()
        {
            Penumbra.AmbientColor = new Color(new Vector3(0.7f));

            CenterTexture = Content.Load<Texture2D>(@"Samples\PenumbraPhysicsEditorLogoBig");
            MoveCamTexture = Content.Load<Texture2D>(@"Samples\MoveCam");
        }

        public void Update(GameTime gameTime, Vector2 mousePosition, bool leftMouseButtonPressed)
        {
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

            DrawBeginCamera2D();

            //Draw sprites here, so they will affected by camera movement

            spriteBatch.Draw(CenterTexture, new Vector2(ViewportWidth / 2, ViewportHeight / 2), Color.White);
            spriteBatch.Draw(MoveCamTexture, new Vector2(-150 + ViewportWidth / 2, ViewportHeight / 2), null, Color.White,
                0f, new Vector2(MoveCamTexture.Width / 2, MoveCamTexture.Height / 2), 1f, SpriteEffects.None, 0f);

            DrawEndCamera2D();

            Penumbra.Draw();

            DrawPhysicsDebugView();
            DrawDisplay();
        }
    }
}
