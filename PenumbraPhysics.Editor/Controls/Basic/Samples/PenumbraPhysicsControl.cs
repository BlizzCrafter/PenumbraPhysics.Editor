using System;
using Microsoft.Xna.Framework;
using PenumbraPhysics.Editor.Classes.Editors.Samples;

namespace PenumbraPhysics.Editor.Controls.Basic
{
    public class PenumbraPhysicsControlSAMPLE : GameControl
    {
        public PenumbraPhysicsEditor Editor;

        protected override void Initialize()
        {
            base.Initialize();

            Editor = new PenumbraPhysicsEditor(_graphicsDeviceService);
            Editor.Initialize();

            VisibleChanged += PenumbraPhysicsControlSAMPLE_VisibleChanged;
        }

        private void PenumbraPhysicsControlSAMPLE_VisibleChanged(object sender, EventArgs e)
        {
            ResetPhysics();
        }

        public override void ClearPhysicsForces()
        {
            Editor.ClearPhysicsForces();
        }

        public override void ResetPhysics()
        {
            Editor.ResetPhysics();
        }

        protected override void Update(GameTime gameTime)
        {
            Editor.Update(gameTime, _MousePosition, LeftMouseButtonPressed);
        }

        protected override void Draw(GameTime gameTime)
        {
            Editor.Draw(gameTime);
        }
    }
}