using Microsoft.Xna.Framework;
using PenumbraPhysics.Editor.Classes.Editors.Samples;
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
        public static bool ShowCursorPosition { get; set; } = false;

        // Show or Hide the cursor position flag
        public static bool ShowPhysicsDebug { get; set; } = true;

        private Control CurrentSourceControl { get; set; }

        private bool MouseIsDownOnButton = false;
        private System.Drawing.Point FirstPointMouseOnButton;

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
        
        //Disable Menu Strip
        private void welcomeUpdateControlSAMPLE1_VisibleChanged(object sender, EventArgs e)
        {
            menuStripEditorFunctions.Enabled = false;
        }
        private void penumbraPhysicsControlSAMPLE1_VisibleChanged_1(object sender, EventArgs e)
        {
            menuStripEditorFunctions.Enabled = false;
        }
        private void tabControl1_VisibleChanged(object sender, EventArgs e)
        {
            menuStripEditorFunctions.Enabled = false;
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
                // Add your Editor Controls in use here
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
                // Add your Editor Controls in use here
                if (CurrentSourceControl is PenumbraPhysicsControlSAMPLE)
                {
                    ((PenumbraPhysicsControlSAMPLE)CurrentSourceControl).ResetPhysics();
                }
            }
        }

        #endregion

        private void placementControlSAMPLE1_VisibleChanged_1(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Visible)
            {
                menuStripEditorFunctions.Tag = placementControlSAMPLE1.Editor;
                menuStripEditorFunctions.Enabled = true;
            }
        }

        private void toolStripMenuItemCreateLight_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ((PlacementEditor)menuStripEditorFunctions.Tag).CreateLight();
            }
        }

        // Camera Control
        private void buttonCameraControl_MouseUp(object sender, MouseEventArgs e)
        {
            MouseIsDownOnButton = false;
        }
        private void buttonCameraControl_MouseDown(object sender, MouseEventArgs e)
        {
            FirstPointMouseOnButton = e.Location;
            MouseIsDownOnButton = true;
        }
        private void buttonCameraControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseIsDownOnButton)
            {
                int xDiff = FirstPointMouseOnButton.X - e.Location.X;
                int yDiff = FirstPointMouseOnButton.Y - e.Location.Y;

                if (menuStripEditorFunctions.Tag is PlacementEditor)
                {
                    ((PlacementEditor)menuStripEditorFunctions.Tag).MoveCam(new Vector2(xDiff, yDiff));
                }

                FirstPointMouseOnButton.X = e.Location.X;
                FirstPointMouseOnButton.Y = e.Location.Y;
            }
        }
    }
}