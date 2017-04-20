namespace PenumbraPhysics.Editor
{
    partial class MainEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainEditor));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageWelcome = new System.Windows.Forms.TabPage();
            this.tabPagePPManipulation = new System.Windows.Forms.TabPage();
            this.tabPagePPPlacement = new System.Windows.Forms.TabPage();
            this.tabPageJustDraw = new System.Windows.Forms.TabPage();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripSplitButtonSettings = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripMenuItemShowFPS = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemShowCursorPosition = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemShowPhysicsDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.penumbraPhysicsControlSAMPLE1 = new PenumbraPhysics.Editor.Controls.Basic.PenumbraPhysicsControlSAMPLE();
            this.updateControlSAMPLE1 = new PenumbraPhysics.Editor.Controls.UpdateControlSAMPLE();
            this.tabControl1.SuspendLayout();
            this.tabPageWelcome.SuspendLayout();
            this.tabPagePPManipulation.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageWelcome);
            this.tabControl1.Controls.Add(this.tabPagePPManipulation);
            this.tabControl1.Controls.Add(this.tabPagePPPlacement);
            this.tabControl1.Controls.Add(this.tabPageJustDraw);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(655, 450);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageWelcome
            // 
            this.tabPageWelcome.Controls.Add(this.penumbraPhysicsControlSAMPLE1);
            this.tabPageWelcome.Location = new System.Drawing.Point(4, 25);
            this.tabPageWelcome.Name = "tabPageWelcome";
            this.tabPageWelcome.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageWelcome.Size = new System.Drawing.Size(647, 421);
            this.tabPageWelcome.TabIndex = 0;
            this.tabPageWelcome.Text = "Welcome!";
            this.tabPageWelcome.UseVisualStyleBackColor = true;
            // 
            // tabPagePPManipulation
            // 
            this.tabPagePPManipulation.Controls.Add(this.updateControlSAMPLE1);
            this.tabPagePPManipulation.Location = new System.Drawing.Point(4, 25);
            this.tabPagePPManipulation.Name = "tabPagePPManipulation";
            this.tabPagePPManipulation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePPManipulation.Size = new System.Drawing.Size(647, 421);
            this.tabPagePPManipulation.TabIndex = 1;
            this.tabPagePPManipulation.Text = "PenumbraPhysics.Manipulation";
            this.tabPagePPManipulation.UseVisualStyleBackColor = true;
            // 
            // tabPagePPPlacement
            // 
            this.tabPagePPPlacement.Location = new System.Drawing.Point(4, 25);
            this.tabPagePPPlacement.Name = "tabPagePPPlacement";
            this.tabPagePPPlacement.Size = new System.Drawing.Size(647, 421);
            this.tabPagePPPlacement.TabIndex = 2;
            this.tabPagePPPlacement.Text = "PenumbraPhysics.Placement";
            this.tabPagePPPlacement.UseVisualStyleBackColor = true;
            // 
            // tabPageJustDraw
            // 
            this.tabPageJustDraw.Location = new System.Drawing.Point(4, 25);
            this.tabPageJustDraw.Name = "tabPageJustDraw";
            this.tabPageJustDraw.Size = new System.Drawing.Size(647, 421);
            this.tabPageJustDraw.TabIndex = 3;
            this.tabPageJustDraw.Text = "Just.Draw.";
            this.tabPageJustDraw.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButtonSettings});
            this.statusStrip.Location = new System.Drawing.Point(0, 450);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(655, 26);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripSplitButtonSettings
            // 
            this.toolStripSplitButtonSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemShowFPS,
            this.toolStripMenuItemShowCursorPosition,
            this.toolStripMenuItemShowPhysicsDebug});
            this.toolStripSplitButtonSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButtonSettings.Image")));
            this.toolStripSplitButtonSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButtonSettings.Name = "toolStripSplitButtonSettings";
            this.toolStripSplitButtonSettings.Size = new System.Drawing.Size(101, 24);
            this.toolStripSplitButtonSettings.Text = "Settings";
            this.toolStripSplitButtonSettings.Click += new System.EventHandler(this.toolStripSplitButtonSettings_Click);
            // 
            // toolStripMenuItemShowFPS
            // 
            this.toolStripMenuItemShowFPS.Checked = true;
            this.toolStripMenuItemShowFPS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemShowFPS.Name = "toolStripMenuItemShowFPS";
            this.toolStripMenuItemShowFPS.Size = new System.Drawing.Size(222, 26);
            this.toolStripMenuItemShowFPS.Text = "Show FPS";
            this.toolStripMenuItemShowFPS.Click += new System.EventHandler(this.toolStripMenuItemShowFPS_Click);
            // 
            // toolStripMenuItemShowCursorPosition
            // 
            this.toolStripMenuItemShowCursorPosition.Checked = true;
            this.toolStripMenuItemShowCursorPosition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemShowCursorPosition.Name = "toolStripMenuItemShowCursorPosition";
            this.toolStripMenuItemShowCursorPosition.Size = new System.Drawing.Size(222, 26);
            this.toolStripMenuItemShowCursorPosition.Text = "Show Cursor Position";
            this.toolStripMenuItemShowCursorPosition.Click += new System.EventHandler(this.toolStripMenuItemShowCursorPosition_Click);
            // 
            // toolStripMenuItemShowPhysicsDebug
            // 
            this.toolStripMenuItemShowPhysicsDebug.Name = "toolStripMenuItemShowPhysicsDebug";
            this.toolStripMenuItemShowPhysicsDebug.Size = new System.Drawing.Size(222, 26);
            this.toolStripMenuItemShowPhysicsDebug.Text = "Show Physics Debug";
            this.toolStripMenuItemShowPhysicsDebug.Click += new System.EventHandler(this.toolStripMenuItemShowPhysicsDebug_Click);
            // 
            // penumbraPhysicsControlSAMPLE1
            // 
            this.penumbraPhysicsControlSAMPLE1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.penumbraPhysicsControlSAMPLE1.Location = new System.Drawing.Point(3, 3);
            this.penumbraPhysicsControlSAMPLE1.Name = "penumbraPhysicsControlSAMPLE1";
            this.penumbraPhysicsControlSAMPLE1.Size = new System.Drawing.Size(641, 415);
            this.penumbraPhysicsControlSAMPLE1.TabIndex = 0;
            this.penumbraPhysicsControlSAMPLE1.Text = "penumbraPhysicsControlSAMPLE1";
            // 
            // updateControlSAMPLE1
            // 
            this.updateControlSAMPLE1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateControlSAMPLE1.Location = new System.Drawing.Point(3, 3);
            this.updateControlSAMPLE1.Name = "updateControlSAMPLE1";
            this.updateControlSAMPLE1.Size = new System.Drawing.Size(641, 415);
            this.updateControlSAMPLE1.TabIndex = 0;
            this.updateControlSAMPLE1.Text = "updateControlSAMPLE1";
            // 
            // MainEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 476);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "MainEditor";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PenumbraPhysics.Editor";
            this.tabControl1.ResumeLayout(false);
            this.tabPageWelcome.ResumeLayout(false);
            this.tabPagePPManipulation.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageWelcome;
        private System.Windows.Forms.TabPage tabPagePPManipulation;
        private Controls.Basic.PenumbraPhysicsControlSAMPLE penumbraPhysicsControlSAMPLE1;
        private Controls.UpdateControlSAMPLE updateControlSAMPLE1;
        private System.Windows.Forms.TabPage tabPagePPPlacement;
        private System.Windows.Forms.TabPage tabPageJustDraw;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButtonSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowFPS;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowCursorPosition;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowPhysicsDebug;
    }
}