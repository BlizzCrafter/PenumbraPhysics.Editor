using FarseerPhysics;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.DebugView;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PenumbraPhysics.Editor.Classes.Editors.Samples
{
    public class PhysicsEditor
    {
        private GameServiceContainer services;
        private GraphicsDevice graphics;
        private ContentManager Content;
        private SpriteBatch spriteBatch;

        private SpriteFont Font;

        private Vector2 GetMousePosition { get; set; }

        // A fixture for our mouse cursor so we can play around with our physics object
        FixedMouseJoint _fixedMouseJoint;

        // The physics body
        Body tBody;
        Texture2D tBodyTexture;
        Vector2 tBodyOrigin;

        // The physical world
        World world;

        DebugViewXNA PhysicsDebugView;

        // 64 Pixel of your screen should be 1 Meter in our physical world
        private float MeterInPixels = 64f;

        // Projection Matrix for PhysicsDebugView
        public static Matrix projection, view;

        // PhysicsDebugView Flag for showing the physical object as a colored shape
        private bool showshapes = true;

        public bool MouseButtonPressed = false;

        private TimeSpan elapsedTime = TimeSpan.Zero;
        private int frameCounter, frameRate;
        private System.Globalization.NumberFormatInfo format;

        public PhysicsEditor(IGraphicsDeviceService graphics)
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
            // Our world for the physics body
            world = new World(Vector2.Zero);

            // Unit conversion rule to get the right position data between simulation space and display space
            ConvertUnits.SetDisplayUnitToSimUnitRatio(MeterInPixels);

            // Initialize the physics debug view
            PhysicsDebugView = new DebugViewXNA(world);
            PhysicsDebugView.LoadContent(this.graphics, Content);
            PhysicsDebugView.RemoveFlags(DebugViewFlags.Controllers);
            PhysicsDebugView.RemoveFlags(DebugViewFlags.Joint);
            PhysicsDebugView.RemoveFlags(DebugViewFlags.Shape);
            PhysicsDebugView.DefaultShapeColor = new Color(255, 255, 0);

            //Loading

            Font = Content.Load<SpriteFont>(@"Font");

            // Loading the texture of the physics object
            tBodyTexture = Content.Load<Texture2D>(@"Samples/object");

            // Creating the physics object
            tBody = CreateComplexBody(world, tBodyTexture, 1f);
            tBody.SleepingAllowed = false;
            tBody.Position = ConvertUnits.ToSimUnits(new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2));
            tBody.BodyType = BodyType.Dynamic;
            tBody.AngularDamping = 2f;
            tBody.Restitution = 1f;
        }

        public void Update(GameTime gameTime, Vector2 mousePosition, bool leftMouseButtonPressed)
        {
            GetMousePosition = mousePosition;

            if (showshapes == true) PhysicsDebugView.AppendFlags(DebugViewFlags.Shape);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.Shape);

            // If left mouse button clicked then create a fixture for physics manipulation
            if (leftMouseButtonPressed && _fixedMouseJoint == null)
            {
                Fixture savedFixture = world.TestPoint(ConvertUnits.ToSimUnits(mousePosition));
                if (savedFixture != null)
                {
                    Body body = savedFixture.Body;
                    _fixedMouseJoint = new FixedMouseJoint(body, ConvertUnits.ToSimUnits(mousePosition));
                    _fixedMouseJoint.MaxForce = 1000.0f * body.Mass;
                    world.AddJoint(_fixedMouseJoint);
                    body.Awake = true;
                }
            }
            // If left mouse button releases then remove the fixture from the world
            if (!leftMouseButtonPressed && _fixedMouseJoint != null)
            {
                world.RemoveJoint(_fixedMouseJoint);
                _fixedMouseJoint = null;
            }
            if (_fixedMouseJoint != null)
                _fixedMouseJoint.WorldAnchorB = ConvertUnits.ToSimUnits(mousePosition);

            // We update the world
            world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

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
            // Matrix projection und Matrix view for PhysicsDebugView
            //
            // Calculate the projection and view adjustments for the debug view
            projection = Matrix.CreateOrthographicOffCenter(0f, graphics.Viewport.Width / MeterInPixels,
                                                             graphics.Viewport.Height / MeterInPixels, 0f, 0f,
                                                             1f);
            view = Matrix.Identity;

            frameCounter++;

            graphics.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            // Draw the texture of the physics body
            spriteBatch.Draw(tBodyTexture, ConvertUnits.ToDisplayUnits(tBody.Position), null,
                        Color.Tomato, tBody.Rotation, tBodyOrigin, 1f, SpriteEffects.None, 0);

            string test = ConvertUnits.ToDisplayUnits(tBody.Position).ToString();

            string fps = string.Format(format, "{0} fps", frameRate);
            string drawString = $"{fps}\n{GetMousePosition}";
            spriteBatch.DrawString(Font, drawString, new Vector2(0, 0), Color.White);

            spriteBatch.End();

            // Draw the physics debug view
            PhysicsDebugView.RenderDebugData(ref projection);
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