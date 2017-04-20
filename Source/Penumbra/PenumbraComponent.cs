﻿using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.ComponentModel;

#pragma warning disable 1591

namespace Penumbra
{
    /// <summary>
    /// GPU based 2D lighting and shadowing engine with penumbra support. Operates with
    /// <see cref="Light"/>s and shadow <see cref="Hull"/>s, where light is a
    /// colored light source which casts shadows on shadow hulls that are outlines of scene
    /// geometry (polygons) impassable by light.
    /// </summary>
    /// <remarks>
    /// Before rendering scene, ensure to call <c>PenumbraComponent.BeginDraw</c> to swap render target
    /// in order for the component to be able to later apply generated lightmap.
    /// </remarks>
    [ToolboxItem(false)]
    public class PenumbraComponent : GraphicsDeviceControlPenumbra
    {
        private readonly PenumbraEngine _engine = new PenumbraEngine();
        private ContentManager _content;
        
        private GraphicsDevice _graphicsDevice { get; set; }

        private bool _initialized;
        private bool _beginDrawCalled;

        /// <summary>
        /// Constructs a new instance of <see cref="PenumbraComponent"/>.
        /// </summary>
        public PenumbraComponent(GraphicsDevice graphicsdevice, ContentManager content)
        {
            _graphicsDevice = graphicsdevice;
            _content = content;
            Initialize();
        }

        /// <summary>
        /// Gets or sets if debug outlines should be drawn for shadows and light sources and
        /// if logging is enabled.
        /// </summary>
        public bool Debug
        {
            get { return _engine.Debug; }
            set { _engine.Debug = value; }
        }

        /// <summary>
        /// Gets or sets the ambient color of the scene. Color is in non-premultiplied format.
        /// </summary>
        public Color AmbientColor
        {
            get { return _engine.AmbientColor; }
            set { _engine.AmbientColor = value; }
        }

        /// <summary>
        /// Gets or sets the custom transformation matrix used by the component.
        /// </summary>
        public Matrix Transform
        {
            get { return _engine.Camera.Custom; }
            set { _engine.Camera.Custom = value; }
        }

        /// <summary>
        /// Gets or sets if this component is used with <see cref="SpriteBatch"/> and its transform should
        /// be automatically applied. Default value is <c>true</c>.
        /// </summary>
        public bool SpriteBatchTransformEnabled
        {
            get { return _engine.Camera.SpriteBatchTransformEnabled; }
            set { _engine.Camera.SpriteBatchTransformEnabled = value; }
        }

        /// <summary>
        /// Gets the list of lights registered with the engine.
        /// </summary>
        public ObservableCollection<Light> Lights => _engine.Lights;

        /// <summary>
        /// Gets the list of shadow hulls registered with the engine.
        /// </summary>
        public ObservableCollection<Hull> Hulls => _engine.Hulls;

        /// <summary>
        /// Gets the diffuse map render target used by Penumbra.
        /// </summary>
        public RenderTarget2D DiffuseMap => _engine.Textures.DiffuseMap;

        /// <summary>
        /// Gets the lightmap render target used by Penumbra.
        /// </summary>
        public RenderTarget2D LightMap => _engine.Textures.Lightmap;
             
        /// <summary>
        /// Sets up the lightmap generation sequence. This should be called before Draw.
        /// </summary>
        public void BeginDraw()
        {
            if (Visible)
            {
                if (!_initialized)
                    throw new InvalidOperationException(
                        $"{nameof(PenumbraComponent)} is not initialized. Make sure to call {nameof(Initialize)} when setting up a game.");

                _engine.PreRender();
                _beginDrawCalled = true;
            }
        }

        /// <inheritdoc />
        private void UnloadContent()
        {
            _engine.Dispose();
            _content?.Dispose();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                UnloadContent();

            base.Dispose(disposing);
        }

        /// <summary>
        /// Explicitly initializes the engine. This should only be called if the
        /// component was not added to the game's components list through <c>Components.Add</c>.
        /// </summary>
        public override void Initialize()
        {
            if (_initialized) return;
            
            _engine.Load(_graphicsDevice,
                _content.Load<Effect>("PenumbraHull"),
                _content.Load<Effect>("PenumbraLight"),
                _content.Load<Effect>("PenumbraShadow"),
                _content.Load<Effect>("PenumbraTexture"));
            _initialized = true;
        }

        public override void Draw()
        {
            if (Visible)
            {
                if (!_beginDrawCalled)
                    throw new InvalidOperationException(
                        $"{nameof(BeginDraw)} must be called before rendering a scene to be lit and calling {nameof(Draw)}.");

                _engine.Render();
                _beginDrawCalled = false;
            }
        }
    }
}