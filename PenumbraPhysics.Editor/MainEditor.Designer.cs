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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.penumbraPhysicsControlSAMPLE1 = new PenumbraPhysics.Editor.Controls.Basic.PenumbraPhysicsControlSAMPLE();
            this.updateControlSAMPLE1 = new PenumbraPhysics.Editor.Controls.UpdateControlSAMPLE();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(654, 450);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.penumbraPhysicsControlSAMPLE1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(646, 421);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.updateControlSAMPLE1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(646, 421);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // penumbraPhysicsControlSAMPLE1
            // 
            this.penumbraPhysicsControlSAMPLE1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.penumbraPhysicsControlSAMPLE1.Location = new System.Drawing.Point(3, 3);
            this.penumbraPhysicsControlSAMPLE1.Name = "penumbraPhysicsControlSAMPLE1";
            this.penumbraPhysicsControlSAMPLE1.Size = new System.Drawing.Size(640, 415);
            this.penumbraPhysicsControlSAMPLE1.TabIndex = 0;
            this.penumbraPhysicsControlSAMPLE1.Text = "penumbraPhysicsControlSAMPLE1";
            // 
            // updateControlSAMPLE1
            // 
            this.updateControlSAMPLE1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.updateControlSAMPLE1.Location = new System.Drawing.Point(3, 3);
            this.updateControlSAMPLE1.Name = "updateControlSAMPLE1";
            this.updateControlSAMPLE1.Size = new System.Drawing.Size(640, 415);
            this.updateControlSAMPLE1.TabIndex = 0;
            this.updateControlSAMPLE1.Text = "updateControlSAMPLE1";
            // 
            // MainEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 450);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainEditor";
            this.Text = "PenumbraPhysics.Editor";
            this.Load += new System.EventHandler(this.MainEditor_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private Controls.Basic.PenumbraPhysicsControlSAMPLE penumbraPhysicsControlSAMPLE1;
        private Controls.UpdateControlSAMPLE updateControlSAMPLE1;
    }
}