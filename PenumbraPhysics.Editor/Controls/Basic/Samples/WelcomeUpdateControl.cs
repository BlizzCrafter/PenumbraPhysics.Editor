using System;
using Microsoft.Xna.Framework;
using PenumbraPhysics.Editor.Classes;

namespace PenumbraPhysics.Editor.Controls
{
    public class WelcomeUpdateControlSAMPLE : GameControl
    {
        private WelcomeUpdateControlSampleEditor Editor;

        protected override void Initialize()
        {
            base.Initialize();

            Editor = new WelcomeUpdateControlSampleEditor(_graphicsDeviceService);
            Editor.Initialize();
        }

        public override void ClearPhysicsForces()
        {
            throw new NotImplementedException();
        }

        public override void ResetPhysics()
        {
            throw new NotImplementedException();
        }

        protected override void Update(GameTime gameTime)
        {
            Editor.Update(gameTime, _MousePosition);
        }

        protected override void Draw(GameTime gameTime)
        {
            Editor.Draw(gameTime);
        }
    }
}