using Microsoft.Xna.Framework;
using PenumbraPhysics.Editor.Classes.Editors.Samples;

namespace PenumbraPhysics.Editor.Controls.Basic
{
    public class PhysicsControlSAMPLE : GameControl
    {
        private PhysicsEditor Editor;

        protected override void Initialize()
        {
            base.Initialize();

            Editor = new PhysicsEditor(_graphicsDeviceService);
            Editor.Initialize();
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
