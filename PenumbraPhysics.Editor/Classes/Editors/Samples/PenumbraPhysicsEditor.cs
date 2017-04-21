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
        private PenumbraComponent Penumbra;

        // Penumbra light
        private Light _light;

        // The physics body
        Body tBody;
        Texture2D tBodyTexture;
        Vector2 tBodyOrigin;
        List<Hull> tBodyHull = new List<Hull>();
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
            tBody = CreateComplexBody(_World, tBodyTexture, tBodyScale);
            tBody.SleepingAllowed = false;
            tBody.Position = ConvertUnits.ToSimUnits(new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2));
            tBody.BodyType = BodyType.Dynamic;
            tBody.AngularDamping = 2f;
            tBody.Restitution = 1f;
            tBody.UserData = new PhysicsBodyFlags()
            {
                StartPosition = tBody.Position,
                StartRotation = tBody.Rotation
            };

            // Create Hulls from the fixtures of the body
            foreach (Fixture f in tBody.FixtureList)
            {
                // Creating the Hull out of the Shape (respectively Vertices) of the fixtures of the physics body
                Hull h = new Hull(((PolygonShape)f.Shape).Vertices);

                // We need to scale the Hull according to our "MetersInPixels-Simulation-Value"
                h.Scale = new Vector2(MeterInPixels);

                // A Hull of Penumbra is set in Display space but the physics body is set in Simulation space
                // Thats why we need to convert the simulation units of the physics object to the display units
                // of the Hull object
                h.Position = ConvertUnits.ToDisplayUnits(tBody.Position);

                // We are adding the new Hull to our physics body hull list
                // This is necessary to update the Hulls in the Update method (see below)
                tBodyHull.Add(h);

                // Adding the Hull to Penumbra
                Penumbra.Hulls.Add(h);
            }
        }

        public void Update(GameTime gameTime, Vector2 mousePosition, bool leftMouseButtonPressed)
        {
            // Animate light position
            _light.Position =
                new Vector2(400f, 240f) + // Offset origin
                new Vector2( // Position around origin
                    (float)Math.Cos(gameTime.TotalGameTime.TotalSeconds),
                    (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds)) * 240f;

            // The rotation and the position of all Hulls will be updated
            // according to the physics body rotation and position
            foreach (Hull h in tBodyHull)
            {
                h.Rotation = tBody.Rotation;
                h.Position = ConvertUnits.ToDisplayUnits(tBody.Position);
            }

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

        /// <summary>
        /// Method for creating complex bodies.
        /// </summary>
        /// <param name="world">The new object will appear in this world</param>
        /// <param name="objectTexture">The new object will get this texture</param>
        /// <param name="Scale">The new object get scaled by this factor</param>
        /// <param name="Algo">The new object get triangulated by this triangulation algorithm</param>
        /// <returns>Returns the complex body</returns>
        public Body CreateComplexBody(World world, Texture2D objectTexture, float Scale,
             TriangulationAlgorithm Algo = TriangulationAlgorithm.Bayazit)
        {
            Body body = null;

            uint[] data = new uint[objectTexture.Width * objectTexture.Height];
            objectTexture.GetData(data);
            Vertices textureVertices = PolygonTools.CreatePolygon(data, objectTexture.Width, false);
            Vector2 centroid = -textureVertices.GetCentroid();
            textureVertices.Translate(ref centroid);
            tBodyOrigin = -centroid;
            textureVertices = SimplifyTools.DouglasPeuckerSimplify(textureVertices, 4f);
            List<Vertices> list = Triangulate.ConvexPartition(textureVertices, Algo);
            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1)) * Scale;
            foreach (Vertices vertices in list)
            {
                vertices.Scale(ref vertScale);
            }

            return body = BodyFactory.CreateCompoundPolygon(world, list, 1f);
        }
    }
}