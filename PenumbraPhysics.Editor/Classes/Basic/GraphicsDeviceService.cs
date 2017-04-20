﻿#region File Description

//-----------------------------------------------------------------------------
// GraphicsDeviceService.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------

#endregion

using System;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;

#pragma warning disable 67

namespace PenumbraPhysics.Editor.Classes.Basic
{
    public class GraphicsDeviceService : IGraphicsDeviceService
    {
        private static GraphicsDeviceService _singletonInstance;
        private static int _referenceCount;
        private readonly PresentationParameters _parameters;

        private GraphicsDeviceService(IntPtr windowHandle, int width, int height)
        {
            _parameters = new PresentationParameters
            {
                BackBufferWidth = Math.Max(width, 1),
                BackBufferHeight = Math.Max(height, 1),
                BackBufferFormat = SurfaceFormat.Color,
                DepthStencilFormat = DepthFormat.Depth24,
                DeviceWindowHandle = windowHandle,
                PresentationInterval = PresentInterval.Immediate,
                IsFullScreen = false
            };
            GraphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter,
                                                         GraphicsProfile.Reach,
                                                         _parameters);
        }

        public GraphicsDevice GraphicsDevice { get; private set; }

        public event EventHandler<EventArgs> DeviceCreated;
        public event EventHandler<EventArgs> DeviceDisposing;
        public event EventHandler<EventArgs> DeviceReset;
        public event EventHandler<EventArgs> DeviceResetting;

        public static GraphicsDeviceService AddRef(IntPtr windowHandle, int width, int height)
        {
            if (Interlocked.Increment(ref _referenceCount) == 1)
            {
                _singletonInstance = new GraphicsDeviceService(windowHandle, width, height);
            }
            return _singletonInstance;
        }

        public void Release(bool disposing)
        {
            if (Interlocked.Decrement(ref _referenceCount) != 0)
                return;
            if (disposing)
            {
                DeviceDisposing?.Invoke(this, EventArgs.Empty);
                GraphicsDevice.Dispose();
            }
            GraphicsDevice = null;
        }

        public void ResetDevice(int width, int height)
        {
            DeviceResetting?.Invoke(this, EventArgs.Empty);
            _parameters.BackBufferWidth = Math.Max(_parameters.BackBufferWidth, width);
            _parameters.BackBufferHeight = Math.Max(_parameters.BackBufferHeight, height);
            GraphicsDevice.Reset(_parameters);
            DeviceReset?.Invoke(this, EventArgs.Empty);
        }
    }
}