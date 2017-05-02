using Microsoft.Xna.Framework;
using Penumbra;
using PenumbraPhysics.Editor.Classes.Basic;
using PenumbraPhysics.Editor.Classes.Editors.Samples;
using PenumbraPhysics.Editor.Controls.Basic.Samples;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PenumbraPhysics.Editor
{
    public partial class MainEditor : Form
    {
        #region DebugViewFlags

        // Show or Hide the cursor position flag
        public static bool ShowPhysicsShapes { get; set; } = false;

        // Show or Hide the PolygonPoints flag
        public static bool ShowPolygonPoints { get; set; } = false;

        // Show or Hide the Joints flag
        public static bool ShowJoints { get; set; } = false;

        // Show or Hide the Controllers flag
        public static bool ShowControllers { get; set; } = false;

        // Show or Hide the Controllers flag
        public static bool ShowContactPoints { get; set; } = false;

        // Show or Hide the CenterOfMass flag
        public static bool ShowCenterOfMass { get; set; } = false;

        // Show or Hide the AABB flag
        public static bool ShowAABB { get; set; } = false;

        // Show or Hide the PerformanceGraph flag
        public static bool ShowPerformanceGraph { get; set; } = false;

        // Show or Hide the DebugPanel flag
        public static bool ShowDebugPanel { get; set; } = false;

        #endregion

        // Show or Hide the FPS display flag
        public static bool ShowFPS { get; set; } = true;

        // Show or Hide the cursor position flag
        public static bool ShowCursorPosition { get; set; } = false;

        // Show or Hide the camera position flag
        public static bool ShowCameraPosition { get; set; } = false;

        // The current visible Editor
        private Control CurrentSourceControl { get; set; }

        // Colors INI (prevent calling the trackbar value changed event when loading colors
        private bool DropDownPenumbraColorLoaded = false;

        // Camera Control
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

        //Visible Changed Events
        private void welcomeUpdateControlSAMPLE1_VisibleChanged(object sender, EventArgs e)
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
                ShowPhysicsShapes = true;

                toolStripStatusLabelNote.Text = "Hold [Left Click] on the [Move Camera] button and move the mouse!";
                toolStripStatusLabelNote.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Control);
                toolStripStatusLabelNote.ForeColor = System.Drawing.Color.Black;
            }
        }
        private void drawControlSAMPLE1_VisibleChanged(object sender, EventArgs e)
        {
            menuStripEditorFunctions.Enabled = false;

            toolStripStatusLabelNote.Text = "Everything in this world is magic and nothing can exist without magic!";
            toolStripStatusLabelNote.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Control);
            toolStripStatusLabelNote.ForeColor = System.Drawing.Color.Black;
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

        #region Penumbra

        // Setting the colors of Penumbra and HexColorIO TextBox
        private void SetPenumbraColors(bool hexEnteredColor = false)
        {
            if (DropDownPenumbraColorLoaded)
            {
                if (CurrentSourceControl != null)
                {
                    PenumbraComponent penumbra = GetPenumbraInContextMenu();

                    if (hexEnteredColor)
                    {
                        System.Drawing.Color rgbColor = System.Drawing.Color.White;
                        try
                        {
                            rgbColor = ColorTranslator.FromHtml(toolStripTextBoxHexColorIO.Text);

                            toolStripColorTrackBarRed._TrackBar.Value = rgbColor.R;
                            toolStripColorTrackBarGreen._TrackBar.Value = rgbColor.G;
                            toolStripColorTrackBarBlue._TrackBar.Value = rgbColor.B;

                            toolStripTextBoxHexColorIO.Text =
                                        ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(
                                            rgbColor.R,
                                            rgbColor.G,
                                            rgbColor.B)).ToString();

                        }
                        catch
                        {
                            if (penumbra != null)
                            {
                                toolStripTextBoxHexColorIO.Text =
                                            ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(
                                                penumbra.AmbientColor.R,
                                                penumbra.AmbientColor.G,
                                                penumbra.AmbientColor.B)).ToString();
                            }
                        }
                    }

                    if (penumbra != null)
                    {
                        penumbra.AmbientColor = new Microsoft.Xna.Framework.Color(
                            toolStripColorTrackBarRed._TrackBar.Value,
                            toolStripColorTrackBarGreen._TrackBar.Value,
                            toolStripColorTrackBarBlue._TrackBar.Value);

                        toolStripTextBoxHexColorIO.Text =
                                    ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(
                                        toolStripColorTrackBarRed._TrackBar.Value,
                                        toolStripColorTrackBarGreen._TrackBar.Value,
                                        toolStripColorTrackBarBlue._TrackBar.Value)).ToString();
                    }
                }
            }
        }

        // Change colors by using the corresponding trackbars
        private void toolStripColorTrackBarRed_ValueChanged(object sender, EventArgs e)
        {
            SetPenumbraColors();
        }
        private void toolStripColorTrackBarGreen_ValueChanged(object sender, EventArgs e)
        {
            SetPenumbraColors();
        }
        private void toolStripColorTrackBarBlue_ValueChanged(object sender, EventArgs e)
        {
            SetPenumbraColors();
        }

        // Event of changeing the text in the HexColorIO TextBox
        private void toolStripTextBoxHexColorIO_TextChanged(object sender, EventArgs e)
        {
            try
            {
                System.Drawing.Color rgbColor = System.Drawing.Color.White;
                rgbColor = ColorTranslator.FromHtml(((ToolStripTextBox)sender).Text);
                ((ToolStripTextBox)sender).BackColor = rgbColor;
                HSLColor hslColor = new HSLColor(240 - rgbColor.GetHue(), rgbColor.GetSaturation(), 240);
                ((ToolStripTextBox)sender).ForeColor = hslColor;

                string trimedHex = ((ToolStripTextBox)sender).Text.TrimStart('#');
                if (trimedHex.Length >= 6) SetPenumbraColors(true);
            }
            catch { }
        }

        // INI trackbar and penumbra colors
        private void ambientColorToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (CurrentSourceControl != null)
            {
                DropDownPenumbraColorLoaded = false;

                PenumbraComponent penumbra = GetPenumbraInContextMenu();

                if (penumbra != null)
                {
                    toolStripColorTrackBarRed._TrackBar.Value = penumbra.AmbientColor.R;
                    toolStripColorTrackBarGreen._TrackBar.Value = penumbra.AmbientColor.G;
                    toolStripColorTrackBarBlue._TrackBar.Value = penumbra.AmbientColor.B;

                    toolStripTextBoxHexColorIO.Text =
                                ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(
                                    penumbra.AmbientColor.R,
                                    penumbra.AmbientColor.G,
                                    penumbra.AmbientColor.B)).ToString();
                }

                DropDownPenumbraColorLoaded = true;
            }
        }

        // Get the current penumbra component through the context menu
        private PenumbraComponent GetPenumbraInContextMenu()
        {
            if (CurrentSourceControl is PenumbraPhysicsControlSAMPLE)
                return ((PenumbraPhysicsControlSAMPLE)CurrentSourceControl).Editor.Penumbra;
            else if (CurrentSourceControl is PlacementControlSAMPLE)
                return ((PlacementControlSAMPLE)CurrentSourceControl).Editor.Penumbra;

            return null;
        }

        #endregion

        #region Physics

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
                else if (CurrentSourceControl is PlacementControlSAMPLE)
                {
                    ((PlacementControlSAMPLE)CurrentSourceControl).ClearPhysicsForces();
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
                else if (CurrentSourceControl is PlacementControlSAMPLE)
                {
                    ((PlacementControlSAMPLE)CurrentSourceControl).ResetPhysics();
                }
            }
        }

        // DebugView
        //
        //Show Physics Shapes
        private void showPhysicsShapesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPhysicsShapes = !ShowPhysicsShapes;
        }

        //Show Polygon Points
        private void toolStripMenuItemShowPolygonPoints_Click(object sender, EventArgs e)
        {
            ShowPolygonPoints = !ShowPolygonPoints;
        }

        //Show Joints
        private void toolStripMenuItemShowJoints_Click(object sender, EventArgs e)
        {
            ShowJoints = !ShowJoints;
        }

        //Show Controllers
        private void toolStripMenuItemShowControllers_Click(object sender, EventArgs e)
        {
            ShowControllers = !ShowControllers;
        }

        //Show Contact Points (Collisions)
        private void toolStripMenuItemShowContactPoints_Click(object sender, EventArgs e)
        {
            ShowContactPoints = !ShowContactPoints;
        }

        //Show Center of Mass
        private void toolStripMenuItemCenterOfMass_Click(object sender, EventArgs e)
        {
            ShowCenterOfMass = !ShowCenterOfMass;
        }

        //Show AABB
        private void toolStripMenuItemShowAABB_Click(object sender, EventArgs e)
        {
            ShowAABB = !ShowAABB;
        }

        //Show PerformanceGraph
        private void toolStripMenuItemShowPerformanceGraph_Click(object sender, EventArgs e)
        {
            ShowPerformanceGraph = !ShowPerformanceGraph;
        }

        //Show DebugPanel
        private void toolStripMenuItemShowDebugPanel_Click(object sender, EventArgs e)
        {
            ShowDebugPanel = !ShowDebugPanel;
        }

        #endregion

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

        // Create a shadow caster
        private void toolStripMenuItemCreateShadowHull_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ((PlacementEditor)menuStripEditorFunctions.Tag).CreateShadowObject();
            }
        }

        // Camera Control
        //
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

        #region ListEditing

        // Remove all objects from the world.
        private void toolStripMenuItemRemoveAllObjects_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ((PlacementEditor)menuStripEditorFunctions.Tag).RemoveAllObjects();
            }
        }

        // Edit the values of all objects in the editor.
        private void toolStripMenuItemListAllObjects_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ObjectList form = new Editor.ObjectList();
                form.SelectedObjects = ((PlacementEditor)menuStripEditorFunctions.Tag).AllObjectList.ToArray();
                form.Show();
            }
        }

        // Edit the values of all light objects in the editor.
        private void toolStripMenuItemListAllLights_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ObjectList form = new Editor.ObjectList();
                form.SelectedObjects = ((PlacementEditor)menuStripEditorFunctions.Tag).LightObjectList.ToArray();
                form.Show();
            }
        }

        // Edit the values of all shadow caster objects in the editor.
        private void toolStripMenuItemListAllShadowCaster_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ObjectList form = new Editor.ObjectList();
                form.SelectedObjects = ((PlacementEditor)menuStripEditorFunctions.Tag).ShadowObjectList.ToArray();
                form.Show();
            }
        }

        // Edit the values of the current selected object in the editor.
        private void toolStripMenuItemEditSelectedObject_Click(object sender, EventArgs e)
        {
            ObjectList form = new Editor.ObjectList();
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                PlacementEditor editor = (PlacementEditor)menuStripEditorFunctions.Tag;

                if (editor.CurrentSelectedObject != null && editor.CurrentSelectedObject.UserData != null)
                {
                    if (editor.CurrentSelectedObject.UserData is PivotBodyFlags)
                    {
                        if (((PivotBodyFlags)editor.CurrentSelectedObject.UserData).ConnectedObject != null &&
                            ((PivotBodyFlags)editor.CurrentSelectedObject.UserData).ConnectedObject.Object != null &&
                            ((PivotBodyFlags)editor.CurrentSelectedObject.UserData).ConnectedObject.Object is Penumbra.Light)
                        {
                            form.SelectedObject =
                                ((PivotBodyFlags)editor.CurrentSelectedObject.UserData).ConnectedObject.Object as Penumbra.Light;
                            form.Show();
                        }
                    }
                    else if (editor.CurrentSelectedObject.UserData is BodyFlags)
                    {
                        form.SelectedObject = editor.CurrentSelectedObject;
                        form.Show();
                    }
                }
                else form.Dispose();
            }
        }

        // Save positions of all objects. This is useful for resetting the game world.
        private void toolStripMenuItemSavePositions_Click(object sender, EventArgs e)
        {
            if (menuStripEditorFunctions.Tag is PlacementEditor)
            {
                ((PlacementEditor)menuStripEditorFunctions.Tag).SaveAllPositions();
            }
        }

        #endregion

        #endregion
    }
}