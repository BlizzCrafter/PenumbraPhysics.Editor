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

        // Show or Hide the camera position flag
        public static bool ShowCameraPosition { get; set; } = false;

        // Show or Hide the cursor position flag
        public static bool ShowPhysicsDebug { get; set; } = false;

        private Control CurrentSourceControl { get; set; }

        private bool MouseIsDownOnButton = false;
        private System.Drawing.Point FirstPointMouseOnButton;

        public MainEditor()
        {
            InitializeComponent();
        }

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
        // Show camera position flag
        private void toolStripMenuItemShowCameraPosition_Click(object sender, EventArgs e)
        {
            ShowCameraPosition = !ShowCameraPosition;
            toolStripMenuItemShowCameraPosition.Checked = ShowCameraPosition;
        }

        // Show Physics Debug flag
        private void toolStripMenuItemShowPhysicsDebug_Click(object sender, EventArgs e)
        {
            ShowPhysicsDebug = !ShowPhysicsDebug;
            toolStripMenuItemShowPhysicsDebug.Checked = ShowPhysicsDebug;
        }
        
        //Visable Changed Events
        private void tabControlWelcome_VisibleChanged(object sender, EventArgs e)
        {
            menuStripEditorFunctions.Enabled = false;

            toolStripStatusLabelNote.Text = "Welcome to the Editor! Click through the tabs to see the different demos.";
            toolStripStatusLabelNote.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Control);
            toolStripStatusLabelNote.ForeColor = System.Drawing.Color.Black;
        }
        private void penumbraPhysicsControlSAMPLE1_VisibleChanged_1(object sender, EventArgs e)
        {
            menuStripEditorFunctions.Enabled = false;
            ShowCursorPosition = true;
            toolStripMenuItemShowCursorPosition.Checked = true;

            toolStripStatusLabelNote.Text = "[Left Click + Drag]: Manipulate Physics Object. [Right Click]: Open Context Menu.";
            toolStripStatusLabelNote.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Control);
            toolStripStatusLabelNote.ForeColor = System.Drawing.Color.Black;
        }
        private void placementControlSAMPLE1_VisibleChanged_1(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Visible)
            {
                menuStripEditorFunctions.Tag = placementControlSAMPLE1.Editor;
                menuStripEditorFunctions.Enabled = true;
                ShowCursorPosition = true;
                toolStripMenuItemShowCursorPosition.Checked = true;
                ShowCameraPosition = true;
                toolStripMenuItemShowCameraPosition.Checked = true;
                ShowPhysicsDebug = true;
                toolStripMenuItemShowPhysicsDebug.Checked = true;

                toolStripStatusLabelNote.Text = "Hold [Left Click] on the [Move Camera] button and move the mouse!";
                toolStripStatusLabelNote.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Control);
                toolStripStatusLabelNote.ForeColor = System.Drawing.Color.Black;
            }
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

        #region Context Menu

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

        #region General
        
        // Create a light
        private void toolStripMenuItemCreateLight_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ((PlacementEditor)menuStripEditorFunctions.Tag).CreateLightObject();
            }
        }

        private void toolStripMenuItemCreateShadowHull_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ((PlacementEditor)menuStripEditorFunctions.Tag).CreateShadowObject();
            }
        }

        // Camera Control
        //Move
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
                    if (((PlacementEditor)menuStripEditorFunctions.Tag).Cam.GetAbsolutPosition.X > 200 &&
                            ((PlacementEditor)menuStripEditorFunctions.Tag).Cam.GetAbsolutPosition.Y > 110)
                    {
                        toolStripStatusLabelNote.Text = "Good! Now create a light or do something else by using the top menu.";
                        toolStripStatusLabelNote.BackColor = System.Drawing.Color.Black;
                        toolStripStatusLabelNote.ForeColor = System.Drawing.Color.Yellow;
                    }
                }

                FirstPointMouseOnButton.X = e.Location.X;
                FirstPointMouseOnButton.Y = e.Location.Y;
            }
        }
        //Reset
        private void toolStripMenuItemResetCamera_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ((PlacementEditor)menuStripEditorFunctions.Tag).ResetCam();
            }
        }

        #endregion

        #endregion

        private void toolStripMenuItemRemoveAllObjects_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ((PlacementEditor)menuStripEditorFunctions.Tag).RemoveAllObjects();
            }
        }

        private void toolStripMenuItemListAllObjects_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ObjectList form = new Editor.ObjectList();
                form.SelectedObjects = ((PlacementEditor)menuStripEditorFunctions.Tag).AllObjectList.ToArray();
                form.Show();
            }
        }

        private void toolStripMenuItemListAllLights_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ObjectList form = new Editor.ObjectList();
                form.SelectedObjects = ((PlacementEditor)menuStripEditorFunctions.Tag).LightObjectList.ToArray();
                form.Show();
            }
        }

        private void toolStripMenuItemListAllShadowCaster_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ObjectList form = new Editor.ObjectList();
                form.SelectedObjects = ((PlacementEditor)menuStripEditorFunctions.Tag).ShadowObjectList.ToArray();
                form.Show();
            }
        }
    }
}