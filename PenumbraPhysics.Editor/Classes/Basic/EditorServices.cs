using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.DebugView;
using FarseerPhysics;

namespace PenumbraPhysics.Editor.Classes.Basic
{
    public class GFXService : IGFXInterface
    {
        public ContentManager Content { get; set; }
        public Vector2 GetMousePosition { get; set; }
        public GraphicsDevice graphics { get; set; }
        public GameServiceContainer services { get; set; }
        public SpriteBatch spriteBatch { get; set; }

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
            spriteBatch = new SpriteBatch(this.graphics);

            Font = Content.Load<SpriteFont>(@"Font");

            Format = new System.Globalization.NumberFormatInfo();
            Format.CurrencyDecimalSeparator = ".";
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

                spriteBatch.End();
            }
        }
    }

    public class PhysicsService : IPhysicsInterface
    {
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
        
        public void ClearPhysicsForces()
        {
            _World.BodyList.ForEach(b => { b.AngularVelocity = 0; b.LinearVelocity = Vector2.Zero; });
            _World.BreakableBodyList.ForEach(b => { b.MainBody.AngularVelocity = 0; b.MainBody.LinearVelocity = Vector2.Zero; });
        }
        public void ResetPhysics()
        {
            _World.BodyList.ForEach(b => 
            {
                b.Position = ((PhysicsBodyFlags)b.UserData).StartPosition;
                b.Rotation = ((PhysicsBodyFlags)b.UserData).StartRotation;
            });
            _World.BreakableBodyList.ForEach(b => 
            {
                b.MainBody.Position = ((PhysicsBodyFlags)b.MainBody.UserData).StartPosition;
                b.MainBody.Rotation = ((PhysicsBodyFlags)b.MainBody.UserData).StartRotation;
            });

            ClearPhysicsForces();
        }

        public void InitializePhysics(GraphicsDevice graphics, ContentManager Content)
        {
            // Catching Viewport dimensions
            ViewportWidth = graphics.Viewport.Width;
            ViewportHeight = graphics.Viewport.Height;

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
            if (MainEditor.ShowPhysicsDebug) PhysicsDebugView.AppendFlags(DebugViewFlags.Shape);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.Shape);

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

        public void DrawPhysicsDebugView()
        {
            if (MainEditor.ShowPhysicsDebug)
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
    }

    public class GFXPhysicsService : IGFXInterface, IPhysicsInterface
    {
        #region GFX Interface

        public ContentManager Content { get; set; }
        public Vector2 GetMousePosition { get; set; }
        public GraphicsDevice graphics { get; set; }
        public GameServiceContainer services { get; set; }
        public SpriteBatch spriteBatch { get; set; }

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
            spriteBatch = new SpriteBatch(this.graphics);

            Font = Content.Load<SpriteFont>(@"Font");

            Format = new System.Globalization.NumberFormatInfo();
            Format.CurrencyDecimalSeparator = ".";
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

                spriteBatch.End();
            }
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

        public void ClearPhysicsForces()
        {
            _World.BodyList.ForEach(b => { b.AngularVelocity = 0; b.LinearVelocity = Vector2.Zero; });
            _World.BreakableBodyList.ForEach(b => { b.MainBody.AngularVelocity = 0; b.MainBody.LinearVelocity = Vector2.Zero; });
        }
        public void ResetPhysics()
        {
            _World.BodyList.ForEach(b =>
            {
                b.Position = ((PhysicsBodyFlags)b.UserData).StartPosition;
                b.Rotation = ((PhysicsBodyFlags)b.UserData).StartRotation;
            });
            _World.BreakableBodyList.ForEach(b =>
            {
                b.MainBody.Position = ((PhysicsBodyFlags)b.MainBody.UserData).StartPosition;
                b.MainBody.Rotation = ((PhysicsBodyFlags)b.MainBody.UserData).StartRotation;
            });

            ClearPhysicsForces();
        }

        public void InitializePhysics(GraphicsDevice graphics, ContentManager Content)
        {
            // Catching Viewport dimensions
            ViewportWidth = graphics.Viewport.Width;
            ViewportHeight = graphics.Viewport.Height;

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
            if (MainEditor.ShowPhysicsDebug) PhysicsDebugView.AppendFlags(DebugViewFlags.Shape);
            else PhysicsDebugView.RemoveFlags(DebugViewFlags.Shape);

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

        public void DrawPhysicsDebugView()
        {
            if (MainEditor.ShowPhysicsDebug)
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

        #endregion
    }
}
