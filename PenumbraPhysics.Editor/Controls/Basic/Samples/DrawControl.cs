using PenumbraPhysics.Editor.Controls;

using Color = Microsoft.Xna.Framework.Color;

namespace PenumbraPhysics.Editor.Controls
{
    public class DrawControlSAMPLE : GraphicsDeviceControl
    {
        protected override void Draw()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
        }

        protected override void Initialize()
        {

        }
    }
}
