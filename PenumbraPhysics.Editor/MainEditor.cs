using System;
using System.Windows.Forms;

namespace PenumbraPhysics.Editor
{
    public partial class MainEditor : Form
    {
        // Show or Hide the FPS display flag
        public static bool ShowFPS { get; set; } = true;

        // Show or Hide the cursor position flag
        public static bool ShowCursorPosition { get; set; } = true;

        // Show or Hide the cursor position flag
        public static bool ShowPhysicsDebug { get; set; } = false;

        public MainEditor()
        {
            InitializeComponent();
        }

        #region Passing Events to underlying Controls

        /* They get triggered inside the corresponding controls! */

        private void physicsSample1_MouseDown(object sender, MouseEventArgs e) { }
        private void physicsSample1_MouseUp(object sender, MouseEventArgs e) { }

        #endregion

        #region Basic Control Events
        
        // Ensures that the drop down opens on button click
        private void toolStripSplitButtonSettings_Click(object sender, EventArgs e)
        {
            toolStripSplitButtonSettings.ShowDropDown();
        }

        // Show FPS display flag
        private void toolStripMenuItemShowFPS_Click(object sender, EventArgs e)
        {
            ShowFPS = !ShowFPS;
            toolStripMenuItemShowFPS.Checked = ShowFPS;
        }

        // Show cursor position flag
        private void toolStripMenuItemShowCursorPosition_Click(object sender, EventArgs e)
        {
            ShowCursorPosition = !ShowCursorPosition;
            toolStripMenuItemShowCursorPosition.Checked = ShowCursorPosition;
        }

        // Show Physics Debug flag
        private void toolStripMenuItemShowPhysicsDebug_Click(object sender, EventArgs e)
        {
            ShowPhysicsDebug = !ShowPhysicsDebug;
            toolStripMenuItemShowPhysicsDebug.Checked = ShowPhysicsDebug;
        }

        #endregion
    }
}