using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PenumbraPhysics.Editor.Classes.Editors.Samples;

namespace PenumbraPhysics.Editor.Controls.Basic
{
    public class PenumbraPhysicsControlSAMPLE : GameControl
    {
        private PenumbraPhysicsEditor Editor;

        protected override void Initialize()
        {
            base.Initialize();

            Editor = new PenumbraPhysicsEditor(_graphicsDeviceService);
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