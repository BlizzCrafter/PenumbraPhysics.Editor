using Microsoft.Xna.Framework;
using PenumbraPhysics.Editor.Classes.Editors.Samples;

namespace PenumbraPhysics.Editor.Controls.Basic.Samples
{
    class PlacementControlSAMPLE : GameControl
    {
        public PlacementEditor Editor;

        protected override void Initialize()
        {
            base.Initialize();

            Editor = new PlacementEditor(_graphicsDeviceService);
            Editor.Initialize();
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
