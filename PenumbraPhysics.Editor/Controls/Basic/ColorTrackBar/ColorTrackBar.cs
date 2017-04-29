using System.Drawing;
using System.Windows.Forms;

namespace PenumbraPhysics.Editor.Controls.Basic.ColorTrackBar
{
    public class ColorTrackBar : TrackBar
    {
        public ColorTrackBar() : base()
        {
            this.Value = 0;
            this.Minimum = 0;
            this.Maximum = 255;

            this.AutoSize = false;
            this.Size = new System.Drawing.Size(100, 20);
            this.TickStyle = TickStyle.None;
            this.BackColor = Color.White;
        }
    }
}
