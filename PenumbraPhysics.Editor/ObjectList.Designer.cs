namespace PenumbraPhysics.Editor
{
    partial class ObjectList
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
            this.propertyGridObjectDetails = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // propertyGridObjectDetails
            // 
            this.propertyGridObjectDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridObjectDetails.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGridObjectDetails.Location = new System.Drawing.Point(0, 0);
            this.propertyGridObjectDetails.Name = "propertyGridObjectDetails";
            this.propertyGridObjectDetails.Size = new System.Drawing.Size(423, 310);
            this.propertyGridObjectDetails.TabIndex = 0;
            this.propertyGridObjectDetails.VisibleChanged += new System.EventHandler(this.propertyGridObjectDetails_VisibleChanged);
            // 
            // ObjectList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(423, 310);
            this.Controls.Add(this.propertyGridObjectDetails);
            this.Name = "ObjectList";
            this.Text = "ObjectList";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGridObjectDetails;
    }
}