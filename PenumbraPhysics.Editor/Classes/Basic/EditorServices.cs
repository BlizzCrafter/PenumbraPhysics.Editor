using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.DebugView;
using FarseerPhysics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PolygonManipulation;
using System.Collections.Generic;
using Penumbra;
using FarseerPhysics.Collision.Shapes;

namespace PenumbraPhysics.Editor.Classes.Basic
{
    public class GFXService : IGFXInterface
    {
        public PenumbraComponent Penumbra { get; set; }

        public ContentManager Content { get; set; }
        public Vector2 GetMousePosition { get; set; }
        public GraphicsDevice graphics { get; set; }
        public GameServiceContainer services { get; set; }
        public SpriteBatch spriteBatch { get; set; }

        public Camera2D Cam { get; set; }

        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;

        //Display
        public SpriteFont Font { get; set; }
        public NumberFormatInfo Format { get; set; }
        public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;
        public int FrameCounter { get; set; }
        public int FrameRate { get; set; }
        
        public void InitializeGFX(IGraphicsDeviceService graphics)
        {
            services = new GameServiceContainer();
            services.AddService<IGraphicsDeviceService>(graphics);

            this.graphics = graphics.GraphicsDevice;

            Content = new ContentManager(services, "Content");
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            if (Penumbra == null) Penumbra = new PenumbraComponent(graphics.GraphicsDevice, Content);

            Font = Content.Load<SpriteFont>(@"Font");

            Format = new System.Globalization.NumberFormatInfo();
            Format.CurrencyDecimalSeparator = ".";

            Cam = new Basic.Camera2D();
            Cam.GetPosition = new Vector2(
                graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
        }

        public void UpdateFrameCounter() => FrameCounter++;
        public void UpdateDisplay(GameTime gameTime, Vector2 mousePosition)
        {
            GetMousePosition = mousePosition;

            ElapsedTime += gameTime.ElapsedGameTime;
            if (ElapsedTime <= TimeSpan.FromSeconds(1)) return;
            ElapsedTime -= TimeSpan.FromSeconds(1);
            FrameRate = FrameCounter;
            FrameCounter = 0;
        }

        public void DrawDisplay()
        {
            if (MainEditor.ShowFPS || MainEditor.ShowCursorPosition)
            {
                spriteBatch.Begin();

                string fps = string.Format(Format, "{0} fps", FrameRate);

                if (MainEditor.ShowFPS)
                {
                    // Draw FPS display
                    spriteBatch.DrawString(Font, fps, new Vector2(5, 0), Color.White);
                }

                if (MainEditor.ShowCursorPosition)
                {
                    // Draw FPS display
                    spriteBatch.DrawString(Font, GetMousePosition.ToString(), new Vector2(
                        5, (!MainEditor.ShowFPS ? 0 : Font.MeasureString(fps).Y)), Color.White);
                }

                if (MainEditor.ShowCameraPosition)
                {
                    // Draw FPS display
                    spriteBatch.DrawString(Font, Cam.GetAbsolutPosition.ToString(), new Vector2(
                        5, (!MainEditor.ShowFPS && !MainEditor.ShowCursorPosition ? 0 :
                        !MainEditor.ShowFPS || !MainEditor.ShowCursorPosition ? Font.MeasureString(fps).Y :
                        Font.MeasureString(fps).Y * 2)), Color.White);
                }

                spriteBatch.End();
            }
        }

        public void DrawBeginCamera2D()
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.NonPremultiplied,
                        null,
                        null,
                        null,
                        null,
                        Cam.get_transformation(graphics));
        }

        public void DrawEndCamera2D()
        {
            spriteBatch.End();
        }

        public void MoveCam(Vector2 amount)
        {
            Cam.Move(new Vector2(amount.X, amount.Y));
            Penumbra.Transform = Matrix.CreateTranslation(amount.X, amount.Y, 0);
        }

        public void ResetCam()
        {
            Penumbra.Transform = Matrix.CreateTranslation(-Cam.GetPosition.X, -Cam.GetPosition.Y, 0);
            Cam.Move(new Vector2(-Cam.GetPosition.X, -Cam.GetPosition.Y));
        }
    }

    public class PhysicsService : IPhysicsInterface
    {
        public PenumbraComponent Penumbra { get; set; }

        // The physical world
        public World _World { get; set; }

        // DebugView for visual debugging the physical world
        public DebugViewXNA PhysicsDebugView { get; set; }

        // Projection Matrix for PhysicsDebugView
        public Matrix Projection;

        // A fixture for our mouse cursor so we can play around with our physics object
        public FixedMouseJoint FixedMouseJoint { get; set; }

        // 64 Pixel of your screen should be 1 Meter in our physical world
        public float MeterInPixels { get; set; } = 64f;

        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        public float CurrentWorldShiftX { get; set; }
        public float CurrentWorldShiftY { get; set; }
        
        public void MoveCam(Vector2 amount)
        {
            _World.ShiftOrigin(new Vector2(ConvertUnits.ToSimUnits(amount.X), ConvertUnits.ToSimUnits(amount.Y)));
            CurrentWorldShiftX += amount.X;
            CurrentWorldShiftY += amount.Y;
        }
        public void ResetCam()
        {
            _World.ShiftOrigin(new Vector2(
                ConvertUnits.ToSimUnits(-CurrentWorldShiftX), ConvertUnits.ToSimUnits(-CurrentWorldShiftY)));
            CurrentWorldShiftX = 0;
            CurrentWorldShiftY = 0;
        }

        public void ClearPhysicsForces()
        {
            _World.BodyList.ForEach(b => { b.AngularVelocity = 0; b.LinearVelocity = Vector2.Zero; });
            _World.BreakableBodyList.ForEach(b => { b.MainBody.AngularVelocity = 0; b.MainBody.LinearVelocity = Vector2.Zero; });
        }
        public void ResetPhysics()
        {
            _World.BodyList.ForEach(b =>
            {
                if (b.UserData != null && b.UserData is BodyFlags)
                {
                    b.Position = ((BodyFlags)b.UserData).StartPosition;
                    b.Rotation = ((BodyFlags)b.UserData).StartRotation;
                }
                else if (b.UserData != null && b.UserData is PivotBodyFlags)
                {
                    b.Position = ((PivotBodyFlags)b.UserData).StartPosition;
                    b.Rotation = ((PivotBodyFlags)b.UserData).StartRotation;
                }
            });
            _World.BreakableBodyList.ForEach(b =>
            {
                if (b.MainBody.UserData != null && b.MainBody.UserData is BodyFlags)
                {
                    b.MainBody.Position = ((BodyFlags)b.MainBody.UserData).StartPosition;
                    b.MainBody.Rotation = ((BodyFlags)b.MainBody.UserData).StartRotation;
                }
                else if (b.MainBody.UserData != null && b.MainBody.UserData is PivotBodyFlags)
                {
                    b.MainBody.Position = ((PivotBodyFlags)b.MainBody.UserData).StartPosition;
                    b.MainBody.Rotation = ((PivotBodyFlags)b.MainBody.UserData).StartRotation;
                }
            });

            ClearPhysicsForces();
        }

        public void SaveAllPositions()
        {
            _World.BodyList.ForEach(b =>
            {
                if (b.UserData != null && b.UserData is BodyFlags)
                {
                    BodyFlags bf = (BodyFlags)b.UserData;
                    bf.StartPosition = b.Position;
                    bf.StartRotation = b.Rotation;
                    b.UserData = bf;
                }
                else if (b.UserData != null && b.UserData is PivotBodyFlags)
                {
                    PivotBodyFlags bf = (PivotBodyFlags)b.UserData;
                    bf.StartPosition = b.Position;
                    bf.StartRotation = b.Rotation;
                    bf.ConnectedObject = ((PivotBodyFlags)b.UserData).ConnectedObject;
                    b.UserData = bf;
                }
            });
            _World.BreakableBodyList.ForEach(b =>
            {
                if (b.MainBody.UserData != null && b.MainBody.UserData is BodyFlags)
                {
                    BodyFlags bf = (BodyFlags)b.MainBody.UserData;
                    bf.StartPosition = b.MainBody.Position;
                    bf.StartRotation = b.MainBody.Rotation;
                    b.MainBody.UserData = bf;
                }
                else if (b.MainBody.UserData != null && b.MainBody.UserData is PivotBodyFlags)
                {
                    PivotBodyFlags bf = (PivotBodyFlags)b.MainBody.UserData;
                    bf.StartPosition = b.MainBody.Position;
                    bf.StartRotation = b.MainBody.Rotation;
                    bf.ConnectedObject = ((PivotBodyFlags)b.MainBody.UserData).ConnectedObject;
                    b.MainBody.UserData = bf;
                }
            });
        }

        public void InitializePhysics(GraphicsDevice graphics, ContentManager Content)
        {
            // Catching Viewport dimensions
            ViewportWidth = graphics.Viewport.Width;
            ViewportHeight = graphics.Viewport.Height;

            if (Penumbra == null) Penumbra = new PenumbraComponent(graphics, Content);

            // Our world for the physics body
            _World = new World(Vector2.Zero);

            // Unit conversion rule to get the right position data between simulation space and display space
            ConvertUnits.SetDisplayUnitToSimUnitRatio(MeterInPixels);

            // Initialize the physics debug view
            PhysicsDebugView = new DebugViewXNA(_World);
            PhysicsDebugView.LoadContent(graphics, Content);
            PhysicsDebugView.RemoveFlags(DebugViewFlags.Controllers);
            PhysicsDebugView.RemoveFlags(DebugViewFlags.Joint);
            PhysicsDebugView.RemoveFlags(DebugViewFlags.Shape);
            PhysicsDebugView.DefaultShapeColor = new Color(255, 255, 0);
        }

        public void UpdatePhysics(GameTime gameTime)
        {
            #region DebugView Update Flags

            if (MainEditor.ShowPhysicsShapes) PhysicsDebugView.AppendFlags(DebugViewFlags.Shape);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.Shape);

            if (MainEditor.ShowPolygonPoints) PhysicsDebugView.AppendFlags(DebugViewFlags.PolygonPoints);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.PolygonPoints);

            if (MainEditor.ShowJoints) PhysicsDebugView.AppendFlags(DebugViewFlags.Joint);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.Joint);

            if (MainEditor.ShowControllers) PhysicsDebugView.AppendFlags(DebugViewFlags.Controllers);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.Controllers);

            if (MainEditor.ShowContactPoints) PhysicsDebugView.AppendFlags(DebugViewFlags.ContactPoints);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.ContactPoints);

            if (MainEditor.ShowCenterOfMass) PhysicsDebugView.AppendFlags(DebugViewFlags.CenterOfMass);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.CenterOfMass);

            if (MainEditor.ShowAABB) PhysicsDebugView.AppendFlags(DebugViewFlags.AABB);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.AABB);

            if (MainEditor.ShowPerformanceGraph) PhysicsDebugView.AppendFlags(DebugViewFlags.PerformanceGraph);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.PerformanceGraph);

            if (MainEditor.ShowDebugPanel) PhysicsDebugView.AppendFlags(DebugViewFlags.DebugPanel);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.DebugPanel);

            #endregion

            _World.BodyList.ForEach(a =>
            {
                if (a.UserData != null)
                {
                    if (a.UserData is PivotBodyFlags)
                    {
                        if (((PivotBodyFlags)a.UserData).ConnectedObject != null)
                        {
                            ((PivotBodyFlags)a.UserData).ConnectedObject.Position = ConvertUnits.ToDisplayUnits(a.Position);
                        }

                        ((PivotBodyFlags)a.UserData).ConnectedObject.Update();
                    }
                }
            });

            // We update the world
            _World.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
        }
        public void UpdatePhysicsManipulation(bool leftMouseButtonPressed, Vector2 mousePosition)
        {
            // If left mouse button clicked then create a fixture for physics manipulation
            if (leftMouseButtonPressed && FixedMouseJoint == null)
            {
                Fixture savedFixture = _World.TestPoint(ConvertUnits.ToSimUnits(mousePosition));
                if (savedFixture != null)
                {
                    Body body = savedFixture.Body;
                    FixedMouseJoint = new FixedMouseJoint(body, ConvertUnits.ToSimUnits(mousePosition));
                    FixedMouseJoint.MaxForce = 1000.0f * body.Mass;
                    _World.AddJoint(FixedMouseJoint);
                    body.Awake = true;
                }
            }
            // If left mouse button releases then remove the fixture from the world
            if (!leftMouseButtonPressed && FixedMouseJoint != null)
            {
                _World.RemoveJoint(FixedMouseJoint);
                FixedMouseJoint = null;
            }
            if (FixedMouseJoint != null)
                FixedMouseJoint.WorldAnchorB = ConvertUnits.ToSimUnits(mousePosition);
        }
        public void UpdateShadowHulls(Body body)
        {
            if (body != null && body.UserData != null)
            {
                if (body.UserData is BodyFlags)
                {
                    if (((BodyFlags)body.UserData).HullList != null)
                    {
                        // The rotation and the position of all Hulls will be updated
                        // according to the physics body rotation and position
                        foreach (Hull h in ((BodyFlags)body.UserData).HullList)
                        {
                            h.Rotation = body.Rotation;
                            h.Position = ConvertUnits.ToDisplayUnits(body.Position);
                        }
                    }
                }
            }
        }
        public void UpdateShadowHulls(List<Body> bodyList)
        {
            if (bodyList != null)
            {
                foreach (Body body in bodyList)
                {
                    if (body.UserData != null && body.UserData is BodyFlags)
                    {
                        if (((BodyFlags)body.UserData).HullList != null)
                        {
                            // The rotation and the position of all Hulls will be updated
                            // according to the physics body rotation and position
                            foreach (Hull h in ((BodyFlags)body.UserData).HullList)
                            {
                                h.Rotation = body.Rotation;
                                h.Position = ConvertUnits.ToDisplayUnits(body.Position);
                            }
                        }
                    }
                }
            }
        }

        public void DrawPhysicsDebugView()
        {
            if (MainEditor.ShowPhysicsShapes)
            {
                // Matrix projection und Matrix view for PhysicsDebugView
                //
                // Calculate the projection and view adjustments for the debug view
                Projection = Matrix.CreateOrthographicOffCenter(0f, ViewportWidth / MeterInPixels,
                                                             ViewportHeight / MeterInPixels, 0f, 0f,
                                                             1f);

                // Draw the physics debug view
                PhysicsDebugView.RenderDebugData(ref Projection);
            }
        }

        /// <summary>
        /// Create borders for the current Editor so that physics objects can't go outside.
        /// </summary>
        public void CreatePhysicalBorders()
        {
            Body DownWall = BodyFactory.CreateRectangle(_World,
                ConvertUnits.ToSimUnits(ViewportWidth), ConvertUnits.ToSimUnits(10), 1f);
            DownWall.BodyType = BodyType.Static;
            DownWall.CollidesWith = Category.All;
            DownWall.CollisionCategories = Category.Cat31;
            DownWall.Position = new Vector2(
                ConvertUnits.ToSimUnits(ViewportWidth / 2), ConvertUnits.ToSimUnits(ViewportHeight + 5));
            DownWall.UserData = "D";

            Body UpWall = BodyFactory.CreateRectangle(_World,
                ConvertUnits.ToSimUnits(ViewportWidth), ConvertUnits.ToSimUnits(10), 1f);
            UpWall.BodyType = BodyType.Static;
            UpWall.CollidesWith = Category.All;
            UpWall.CollisionCategories = Category.Cat31;
            UpWall.Position = new Vector2(
                ConvertUnits.ToSimUnits(ViewportWidth / 2), ConvertUnits.ToSimUnits(-4));
            UpWall.UserData = "U";

            Body LeftWall = BodyFactory.CreateRectangle(_World,
                ConvertUnits.ToSimUnits(10), ConvertUnits.ToSimUnits(ViewportHeight), 1f);
            LeftWall.BodyType = BodyType.Static;
            LeftWall.CollidesWith = Category.All;
            LeftWall.CollisionCategories = Category.Cat31;
            LeftWall.Position = new Vector2(
                ConvertUnits.ToSimUnits(-4), ConvertUnits.ToSimUnits(ViewportHeight / 2));
            LeftWall.UserData = "L";

            Body RightWall = BodyFactory.CreateRectangle(_World,
                ConvertUnits.ToSimUnits(10), ConvertUnits.ToSimUnits(ViewportHeight), 1f);
            RightWall.BodyType = BodyType.Static;
            RightWall.CollidesWith = Category.All;
            RightWall.CollisionCategories = Category.Cat31;
            RightWall.Position = new Vector2(
                ConvertUnits.ToSimUnits(ViewportWidth + 4), ConvertUnits.ToSimUnits(ViewportHeight / 2));
            RightWall.UserData = "R";
        }

        /// <summary>
        /// Method for creating complex bodies.
        /// </summary>
        /// <param name="world">The new object will appear in this world</param>
        /// <param name="objectTexture">The new object will get this texture</param>
        /// <param name="Scale">The new object get scaled by this factor</param>
        /// <param name="Algorithm">The new object get triangulated by this triangulation algorithm</param>
        /// <returns>Returns the complex body</returns>
        public Body CreateComplexBody(World world, Texture2D objectTexture, float Scale, out Vector2 Origin,
            TriangulationAlgorithm Algorithm = TriangulationAlgorithm.Bayazit)
        {
            Body body = null;

            uint[] data = new uint[objectTexture.Width * objectTexture.Height];
            objectTexture.GetData(data);
            Vertices textureVertices = PolygonTools.CreatePolygon(data, objectTexture.Width, false);
            Vector2 centroid = -textureVertices.GetCentroid();
            textureVertices.Translate(ref centroid);
            Origin = -centroid;
            textureVertices = SimplifyTools.CollinearSimplify(textureVertices, 4f);
            List<Vertices> list = Triangulate.ConvexPartition(textureVertices, Algorithm);
            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1)) * Scale;
            foreach (Vertices vertices in list)
            {
                vertices.Scale(ref vertScale);
            }

            return body = BodyFactory.CreateCompoundPolygon(world, list, 1f);
        }

        /// <summary>
        /// Method for creating shadow hulls out of a physics body.
        /// </summary>
        /// <param name="body">Out of this Body the shadow Hulls will be generated.</param>
        public void CreateShadowHulls(Body body)
        {
            if (body != null && body.UserData != null)
            {
                if (body.UserData is BodyFlags)
                {
                    if (((BodyFlags)body.UserData).HullList != null)
                    {
                        // Create Hulls from the fixtures of the body
                        foreach (Fixture f in body.FixtureList)
                        {
                            // Creating the Hull out of the Shape (respectively Vertices) of the fixtures of the physics body
                            Hull h = new Hull(((PolygonShape)f.Shape).Vertices);

                            // We need to scale the Hull according to our "MetersInPixels-Simulation-Value"
                            h.Scale = new Vector2(MeterInPixels + ((BodyFlags)body.UserData).ShadowHullScale);

                            // A Hull of Penumbra is set in Display space but the physics body is set in Simulation space
                            // Thats why we need to convert the simulation units of the physics object to the display units
                            // of the Hull object
                            h.Position = ConvertUnits.ToDisplayUnits(body.Position);

                            // We are adding the new Hull to our physics body hull list
                            // This is necessary to update the Hulls in the Update method (see below)
                            ((BodyFlags)body.UserData).HullList.Add(h);

                            // Adding the Hull to Penumbra
                            Penumbra.Hulls.Add(h);
                        }
                    }
                }
            }
        }
    }
    
    public class GFXPhysicsService : IGFXInterface, IPhysicsInterface
    {
        #region GFX Interface

        public PenumbraComponent Penumbra { get; set; }

        public ContentManager Content { get; set; }
        public Vector2 GetMousePosition { get; set; }
        public GraphicsDevice graphics { get; set; }
        public GameServiceContainer services { get; set; }
        public SpriteBatch spriteBatch { get; set; }

        public Camera2D Cam { get; set; }

        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;

        public float CurrentWorldShiftX { get; set; }
        public float CurrentWorldShiftY { get; set; }

        //Display
        public SpriteFont Font { get; set; }
        public NumberFormatInfo Format { get; set; }
        public TimeSpan ElapsedTime { get; set; } = TimeSpan.Zero;
        public int FrameCounter { get; set; }
        public int FrameRate { get; set; }

        public void InitializeGFX(IGraphicsDeviceService graphics)
        {
            services = new GameServiceContainer();
            services.AddService<IGraphicsDeviceService>(graphics);

            this.graphics = graphics.GraphicsDevice;

            Content = new ContentManager(services, "Content");
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);

            if (Penumbra == null) Penumbra = new PenumbraComponent(graphics.GraphicsDevice, Content);

            Font = Content.Load<SpriteFont>(@"Font");

            Format = new System.Globalization.NumberFormatInfo();
            Format.CurrencyDecimalSeparator = ".";
            
            Cam = new Basic.Camera2D();
            Cam.GetPosition = new Vector2(
                graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height / 2);
        }

        public void UpdateFrameCounter() => FrameCounter++;
        public void UpdateDisplay(GameTime gameTime, Vector2 mousePosition)
        {
            GetMousePosition = mousePosition;

            ElapsedTime += gameTime.ElapsedGameTime;
            if (ElapsedTime <= TimeSpan.FromSeconds(1)) return;
            ElapsedTime -= TimeSpan.FromSeconds(1);
            FrameRate = FrameCounter;
            FrameCounter = 0;
        }

        public void DrawDisplay()
        {
            if (MainEditor.ShowFPS || MainEditor.ShowCursorPosition || MainEditor.ShowCameraPosition)
            {
                spriteBatch.Begin();

                string fps = string.Format(Format, "{0} fps", FrameRate);

                if (MainEditor.ShowFPS)
                {
                    // Draw FPS display
                    spriteBatch.DrawString(Font, fps, new Vector2(5, 0), Color.White);
                }

                if (MainEditor.ShowCursorPosition)
                {
                    // Draw FPS display
                    spriteBatch.DrawString(Font, GetMousePosition.ToString(), new Vector2(
                        5, (!MainEditor.ShowFPS ? 0 : Font.MeasureString(fps).Y)), Color.White);
                }

                if (MainEditor.ShowCameraPosition)
                {
                    // Draw FPS display
                    spriteBatch.DrawString(Font, Cam.GetAbsolutPosition.ToString(), new Vector2(
                        5, (!MainEditor.ShowFPS && !MainEditor.ShowCursorPosition ? 0 :
                        !MainEditor.ShowFPS || !MainEditor.ShowCursorPosition ? Font.MeasureString(fps).Y :
                        Font.MeasureString(fps).Y * 2)), Color.White);
                }

                spriteBatch.End();
            }
        }

        public void DrawBeginCamera2D()
        {
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.NonPremultiplied,
                        null,
                        null,
                        null,
                        null,
                        Cam.get_transformation(graphics));
        }

        public void DrawEndCamera2D()
        {
            spriteBatch.End();
        }

        #endregion

        #region Physics Interface

        // The physical world
        public World _World { get; set; }

        // DebugView for visual debugging the physical world
        public DebugViewXNA PhysicsDebugView { get; set; }

        // Projection Matrix for PhysicsDebugView
        public Matrix Projection;

        // A fixture for our mouse cursor so we can play around with our physics object
        public FixedMouseJoint FixedMouseJoint { get; set; }

        // 64 Pixel of your screen should be 1 Meter in our physical world
        public float MeterInPixels { get; set; } = 64f;

        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }
        
        public void MoveCam(Vector2 amount)
        {
            Cam.Move(new Vector2(amount.X, amount.Y));
            Penumbra.Transform = Matrix.CreateTranslation(amount.X, amount.Y, 0);
            _World.ShiftOrigin(new Vector2(ConvertUnits.ToSimUnits(amount.X), ConvertUnits.ToSimUnits(amount.Y)));
            CurrentWorldShiftX += amount.X;
            CurrentWorldShiftY += amount.Y;
        }

        public void ResetCam()
        {
            Cam.Move(new Vector2(-CurrentWorldShiftX, -CurrentWorldShiftY));
            Penumbra.Transform = Matrix.CreateTranslation(-CurrentWorldShiftX, -CurrentWorldShiftY, 0);
            Penumbra.Transform = Matrix.CreateTranslation(0.1f, 0.1f, 0); // Update the Matrix in Penumbra by adding a small value
            _World.ShiftOrigin(new Vector2(
                ConvertUnits.ToSimUnits(-CurrentWorldShiftX), ConvertUnits.ToSimUnits(-CurrentWorldShiftY)));
            CurrentWorldShiftX = 0;
            CurrentWorldShiftY = 0;
        }

        public void ClearPhysicsForces()
        {
            _World.BodyList.ForEach(b => { b.AngularVelocity = 0; b.LinearVelocity = Vector2.Zero; });
            _World.BreakableBodyList.ForEach(b => { b.MainBody.AngularVelocity = 0; b.MainBody.LinearVelocity = Vector2.Zero; });
        }
        public void ResetPhysics()
        {
            _World.BodyList.ForEach(b =>
            {
                if (b.UserData != null && b.UserData is BodyFlags)
                {
                    b.Position = ((BodyFlags)b.UserData).StartPosition;
                    b.Rotation = ((BodyFlags)b.UserData).StartRotation;
                }
                else if (b.UserData != null && b.UserData is PivotBodyFlags)
                {
                    b.Position = ((PivotBodyFlags)b.UserData).StartPosition;
                    b.Rotation = ((PivotBodyFlags)b.UserData).StartRotation;
                }
            });
            _World.BreakableBodyList.ForEach(b =>
            {
                if (b.MainBody.UserData != null && b.MainBody.UserData is BodyFlags)
                {
                    b.MainBody.Position = ((BodyFlags)b.MainBody.UserData).StartPosition;
                    b.MainBody.Rotation = ((BodyFlags)b.MainBody.UserData).StartRotation;
                }
                else if (b.MainBody.UserData != null && b.MainBody.UserData is PivotBodyFlags)
                {
                    b.MainBody.Position = ((PivotBodyFlags)b.MainBody.UserData).StartPosition;
                    b.MainBody.Rotation = ((PivotBodyFlags)b.MainBody.UserData).StartRotation;
                }
            });

            ClearPhysicsForces();
        }

        public void SaveAllPositions()
        {
            _World.BodyList.ForEach(b =>
            {
                if (b.UserData != null && b.UserData is BodyFlags)
                {
                    BodyFlags bf = (BodyFlags)b.UserData;
                    bf.StartPosition = b.Position;
                    bf.StartRotation = b.Rotation;
                    b.UserData = bf;
                }
                else if (b.UserData != null && b.UserData is PivotBodyFlags)
                {
                    PivotBodyFlags bf = (PivotBodyFlags)b.UserData;
                    bf.StartPosition = b.Position;
                    bf.StartRotation = b.Rotation;
                    bf.ConnectedObject = ((PivotBodyFlags)b.UserData).ConnectedObject;
                    b.UserData = bf;
                }
            });
            _World.BreakableBodyList.ForEach(b =>
            {
                if (b.MainBody.UserData != null && b.MainBody.UserData is BodyFlags)
                {
                    BodyFlags bf = (BodyFlags)b.MainBody.UserData;
                    bf.StartPosition = b.MainBody.Position;
                    bf.StartRotation = b.MainBody.Rotation;
                    b.MainBody.UserData = bf;
                }
                else if (b.MainBody.UserData != null && b.MainBody.UserData is PivotBodyFlags)
                {
                    PivotBodyFlags bf = (PivotBodyFlags)b.MainBody.UserData;
                    bf.StartPosition = b.MainBody.Position;
                    bf.StartRotation = b.MainBody.Rotation;
                    bf.ConnectedObject = ((PivotBodyFlags)b.MainBody.UserData).ConnectedObject;
                    b.MainBody.UserData = bf;
                }
            });
        }

        public void InitializePhysics(GraphicsDevice graphics, ContentManager Content)
        {
            // Catching Viewport dimensions
            ViewportWidth = graphics.Viewport.Width;
            ViewportHeight = graphics.Viewport.Height;

            if (Penumbra == null) Penumbra = new PenumbraComponent(graphics, Content);

            // Our world for the physics body
            _World = new World(Vector2.Zero);

            // Unit conversion rule to get the right position data between simulation space and display space
            ConvertUnits.SetDisplayUnitToSimUnitRatio(MeterInPixels);

            // Initialize the physics debug view
            PhysicsDebugView = new DebugViewXNA(_World);
            PhysicsDebugView.LoadContent(graphics, Content);
            PhysicsDebugView.RemoveFlags(DebugViewFlags.Controllers);
            PhysicsDebugView.RemoveFlags(DebugViewFlags.Joint);
            PhysicsDebugView.RemoveFlags(DebugViewFlags.Shape);
            PhysicsDebugView.DefaultShapeColor = new Color(255, 255, 0);
        }

        public void UpdatePhysics(GameTime gameTime)
        {
            #region DebugView Update Flags

            if (MainEditor.ShowPhysicsShapes) PhysicsDebugView.AppendFlags(DebugViewFlags.Shape);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.Shape);

            if (MainEditor.ShowPolygonPoints) PhysicsDebugView.AppendFlags(DebugViewFlags.PolygonPoints);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.PolygonPoints);

            if (MainEditor.ShowJoints) PhysicsDebugView.AppendFlags(DebugViewFlags.Joint);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.Joint);

            if (MainEditor.ShowControllers) PhysicsDebugView.AppendFlags(DebugViewFlags.Controllers);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.Controllers);

            if (MainEditor.ShowContactPoints) PhysicsDebugView.AppendFlags(DebugViewFlags.ContactPoints);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.ContactPoints);

            if (MainEditor.ShowCenterOfMass) PhysicsDebugView.AppendFlags(DebugViewFlags.CenterOfMass);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.CenterOfMass);

            if (MainEditor.ShowAABB) PhysicsDebugView.AppendFlags(DebugViewFlags.AABB);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.AABB);

            if (MainEditor.ShowPerformanceGraph) PhysicsDebugView.AppendFlags(DebugViewFlags.PerformanceGraph);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.PerformanceGraph);

            if (MainEditor.ShowDebugPanel) PhysicsDebugView.AppendFlags(DebugViewFlags.DebugPanel);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.DebugPanel);

            #endregion
            
            _World.BodyList.ForEach(a =>
            {
                if (a.UserData != null)
                {
                    if (a.UserData is PivotBodyFlags)
                    {
                        if (((PivotBodyFlags)a.UserData).ConnectedObject != null)
                        {
                            ((PivotBodyFlags)a.UserData).ConnectedObject.Position = ConvertUnits.ToDisplayUnits(a.Position);
                        }

                        ((PivotBodyFlags)a.UserData).ConnectedObject.Update();
                    }
                }
            });

            // We update the world
            _World.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);
        }
        public void UpdatePhysicsManipulation(bool leftMouseButtonPressed, Vector2 mousePosition)
        {
            // If left mouse button clicked then create a fixture for physics manipulation
            if (leftMouseButtonPressed && FixedMouseJoint == null)
            {
                Fixture savedFixture = _World.TestPoint(ConvertUnits.ToSimUnits(mousePosition));
                if (savedFixture != null)
                {
                    Body body = savedFixture.Body;
                    if (body.BodyType == BodyType.Dynamic)
                    {
                        FixedMouseJoint = new FixedMouseJoint(body, ConvertUnits.ToSimUnits(mousePosition));
                        FixedMouseJoint.MaxForce = 1000.0f * body.Mass;
                        _World.AddJoint(FixedMouseJoint);
                        body.Awake = true;
                    }
                }
            }
            // If left mouse button releases then remove the fixture from the world
            if (!leftMouseButtonPressed && FixedMouseJoint != null)
            {
                _World.RemoveJoint(FixedMouseJoint);
                FixedMouseJoint = null;
            }
            if (FixedMouseJoint != null)
                FixedMouseJoint.WorldAnchorB = ConvertUnits.ToSimUnits(mousePosition);
        }
        public void UpdateShadowHulls(Body body)
        {
            if (body != null && body.UserData != null)
            {
                if (body.UserData is BodyFlags)
                {
                    if (((BodyFlags)body.UserData).HullList != null)
                    {
                        // The rotation and the position of all Hulls will be updated
                        // according to the physics body rotation and position
                        foreach (Hull h in ((BodyFlags)body.UserData).HullList)
                        {
                            h.Rotation = body.Rotation;
                            h.Position = ConvertUnits.ToDisplayUnits(body.Position);
                        }
                    }
                }
            }
        }
        public void UpdateShadowHulls(List<Body> bodyList)
        {
            if (bodyList != null)
            {
                foreach (Body body in bodyList)
                {
                    if (body.UserData != null && body.UserData is BodyFlags)
                    {
                        if (body.ResetShadowHulls)
                        {
                            body.ResetShadowHulls = false;

                            ((BodyFlags)body.UserData).HullList.ForEach(h => Penumbra.Hulls.Remove(h));
                            ((BodyFlags)body.UserData).HullList.Clear();
                            CreateShadowHulls(body);
                        }
                        else if (((BodyFlags)body.UserData).HullList != null)
                        {
                            // The rotation and the position of all Hulls will be updated
                            // according to the physics body rotation and position
                            foreach (Hull h in ((BodyFlags)body.UserData).HullList)
                            {
                                h.Rotation = body.Rotation;
                                h.Position = ConvertUnits.ToDisplayUnits(body.Position);
                            }
                        }
                    }
                }
            }
        }
        
        public void DrawPhysicsDebugView()
        {
            if (MainEditor.ShowPhysicsShapes)
            {
                // Matrix projection und Matrix view for PhysicsDebugView
                //
                // Calculate the projection and view adjustments for the debug view
                Projection = Matrix.CreateOrthographicOffCenter(0f, ViewportWidth / MeterInPixels,
                                                                 ViewportHeight / MeterInPixels, 0f, 0f,
                                                                 1f);

                // Draw the physics debug view
                PhysicsDebugView.RenderDebugData(ref Projection);
            }
        }

        /// <summary>
        /// Create borders for the current Editor so that physics objects can't go outside.
        /// </summary>
        public void CreatePhysicalBorders()
        {
            Body DownWall = BodyFactory.CreateRectangle(_World,
                ConvertUnits.ToSimUnits(ViewportWidth), ConvertUnits.ToSimUnits(10), 1f);
            DownWall.BodyType = BodyType.Static;
            DownWall.Position = new Vector2(
                ConvertUnits.ToSimUnits(ViewportWidth / 2), ConvertUnits.ToSimUnits(ViewportHeight + 5));
            DownWall.UserData = "D";

            Body UpWall = BodyFactory.CreateRectangle(_World,
                ConvertUnits.ToSimUnits(ViewportWidth), ConvertUnits.ToSimUnits(10), 1f);
            UpWall.BodyType = BodyType.Static;
            UpWall.Position = new Vector2(
                ConvertUnits.ToSimUnits(ViewportWidth / 2), ConvertUnits.ToSimUnits(-4));
            UpWall.UserData = "U";

            Body LeftWall = BodyFactory.CreateRectangle(_World,
                ConvertUnits.ToSimUnits(10), ConvertUnits.ToSimUnits(ViewportHeight), 1f);
            LeftWall.BodyType = BodyType.Static;
            LeftWall.Position = new Vector2(
                ConvertUnits.ToSimUnits(-4), ConvertUnits.ToSimUnits(ViewportHeight / 2));
            LeftWall.UserData = "L";

            Body RightWall = BodyFactory.CreateRectangle(_World,
                ConvertUnits.ToSimUnits(10), ConvertUnits.ToSimUnits(ViewportHeight), 1f);
            RightWall.BodyType = BodyType.Static;
            RightWall.Position = new Vector2(
                ConvertUnits.ToSimUnits(ViewportWidth + 4), ConvertUnits.ToSimUnits(ViewportHeight / 2));
            RightWall.UserData = "R";
        }

        /// <summary>
        /// Method for creating complex bodies.
        /// </summary>
        /// <param name="world">The new object will appear in this world</param>
        /// <param name="objectTexture">The new object will get this texture</param>
        /// <param name="Scale">The new object get scaled by this factor</param>
        /// <param name="Algorithm">The new object get triangulated by this triangulation algorithm</param>
        /// <returns>Returns the complex body</returns>
        public Body CreateComplexBody(World world, Texture2D objectTexture, float Scale, out Vector2 Origin,
            TriangulationAlgorithm Algorithm = TriangulationAlgorithm.Bayazit)
        {
            Body body = null;

            uint[] data = new uint[objectTexture.Width * objectTexture.Height];
            objectTexture.GetData(data);
            Vertices textureVertices = PolygonTools.CreatePolygon(data, objectTexture.Width, false);
            Vector2 centroid = -textureVertices.GetCentroid();
            textureVertices.Translate(ref centroid);
            Origin = -centroid;
            textureVertices = SimplifyTools.CollinearSimplify(textureVertices, 4f);
            List<Vertices> list = Triangulate.ConvexPartition(textureVertices, Algorithm);
            Vector2 vertScale = new Vector2(ConvertUnits.ToSimUnits(1)) * Scale;
            foreach (Vertices vertices in list)
            {
                vertices.Scale(ref vertScale);
            }

            return body = BodyFactory.CreateCompoundPolygon(world, list, 1f);
        }

        /// <summary>
        /// Method for creating shadow hulls out of a physics body.
        /// </summary>
        /// <param name="body">Out of this Body the shadow Hulls will be generated.</param>
        public void CreateShadowHulls(Body body)
        {
            if (body != null && body.UserData != null)
            {
                if (body.UserData is BodyFlags)
                {
                    if (((BodyFlags)body.UserData).HullList != null)
                    {
                        // Create Hulls from the fixtures of the body
                        foreach (Fixture f in body.FixtureList)
                        {
                            // Creating the Hull out of the Shape (respectively Vertices) of the fixtures of the physics body
                            Hull h = new Hull(((PolygonShape)f.Shape).Vertices);

                            // We need to scale the Hull according to our "MetersInPixels-Simulation-Value"
                            h.Scale = new Vector2(MeterInPixels + ((BodyFlags)body.UserData).ShadowHullScale);

                            // A Hull of Penumbra is set in Display space but the physics body is set in Simulation space
                            // Thats why we need to convert the simulation units of the physics object to the display units
                            // of the Hull object
                            h.Position = ConvertUnits.ToDisplayUnits(body.Position);

                            // We are adding the new Hull to our physics body hull list
                            // This is necessary to update the Hulls in the Update method (see below)
                            ((BodyFlags)body.UserData).HullList.Add(h);

                            // Adding the Hull to Penumbra
                            Penumbra.Hulls.Add(h);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
