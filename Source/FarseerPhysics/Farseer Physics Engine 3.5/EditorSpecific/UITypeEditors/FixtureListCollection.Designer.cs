namespace PenumbraPhysics.Editor.UITypeEditors
{
    partial class FixtureListCollection
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
            this.propertyGridFixtureList = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // propertyGridFixtureList
            // 
            this.propertyGridFixtureList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridFixtureList.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGridFixtureList.Location = new System.Drawing.Point(0, 0);
            this.propertyGridFixtureList.Name = "propertyGridFixtureList";
            this.propertyGridFixtureList.Size = new System.Drawing.Size(457, 485);
            this.propertyGridFixtureList.TabIndex = 0;
            this.propertyGridFixtureList.VisibleChanged += new System.EventHandler(this.propertyGridFixtureList_VisibleChanged);
            // 
            // FixtureListCollection
            // 
            this.ClientSize = new System.Drawing.Size(457, 485);
            this.Controls.Add(this.propertyGridFixtureList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FixtureListCollection";
            this.Text = "FixtureList";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGridFixtureList;
    }
}