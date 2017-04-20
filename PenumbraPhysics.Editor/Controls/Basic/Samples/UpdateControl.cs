using Microsoft.Xna.Framework;
using PenumbraPhysics.Editor.Classes;

namespace PenumbraPhysics.Editor.Controls
{
    public class UpdateControlSAMPLE : GameControl
    {
        private UpdateControlSampleEditor Editor;

        protected override void Initialize()
        {
            base.Initialize();

            Editor = new UpdateControlSampleEditor(_graphicsDeviceService);
            Editor.Initialize();
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