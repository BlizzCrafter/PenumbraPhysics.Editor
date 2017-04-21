using PenumbraPhysics.Editor.Controls.Basic;
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

        private Control CurrentSourceControl { get; set; }

        public MainEditor()
        {
            InitializeComponent();
        }

        #region Dispose Messages

        // Dispose PhysicsManippulationSample Message
        private void buttonCloseManipulationSampleMessage_Click(object sender, EventArgs e)
        {
            richTextBoxManipulationSample.Dispose();
            buttonCloseManipulationSampleMessage.Dispose();
        }

        #endregion

        #region Passing Events to underlying Controls

        /* They get triggered inside the corresponding controls! */

        private void physicsSample1_MouseDown(object sender, MouseEventArgs e) { }
        private void physicsSample1_MouseUp(object sender, MouseEventArgs e) { }
        private void penumbraPhysicsControlSAMPLE1_VisibleChanged(object sender, EventArgs e) { }

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

        // Get the underlying source control of the context menu strip
        // We are catching this separately, because there is a bug when catching this
        // from a sub owner
        private void contextMenuStripPenumbraPhysicsMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            CurrentSourceControl = ((ContextMenuStrip)sender).SourceControl;
        }

        #endregion

        #region Editor Events

        // Clear Physics Forces
        private void toolStripMenuItemClearPhysicsForces_Click(object sender, EventArgs e)
        {
            if (CurrentSourceControl != null)
            {
                if (CurrentSourceControl is PenumbraPhysicsControlSAMPLE)
                {
                    ((PenumbraPhysicsControlSAMPLE)CurrentSourceControl).ClearPhysicsForces();
                }
            }
        }

        // Reset Physics
        private void ToolStripMenuItemResetAll_Click(object sender, EventArgs e)
        {
            if (CurrentSourceControl != null)
            {
                if (CurrentSourceControl is PenumbraPhysicsControlSAMPLE)
                {
                    ((PenumbraPhysicsControlSAMPLE)CurrentSourceControl).ResetPhysics();
                }
            }
        }

        #endregion

    }
}