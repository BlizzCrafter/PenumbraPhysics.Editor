using System;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace PenumbraPhysics.Editor.Controls.Basic.ColorTrackBar
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.All)]
    public partial class ToolStripColorTrackBar : ToolStripControlHost
    {
        public ColorTrackBar _TrackBar;
        public event EventHandler ValueChanged;

        public ToolStripColorTrackBar() : base(new ColorTrackBar())
        {
            InitializeComponent();

            _TrackBar = (ColorTrackBar)this.Control;
            _TrackBar.ValueChanged += _TrackBar_ValueChanged;
        }

        private void _TrackBar_ValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }
    }
}
