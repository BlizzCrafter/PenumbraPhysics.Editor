namespace PenumbraPhysics.Editor.UITypeEditors
{
    partial class VerticesCollection
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
            this.propertyGridVerticesCollection = new System.Windows.Forms.PropertyGrid();
            this.SuspendLayout();
            // 
            // propertyGridVerticesCollection
            // 
            this.propertyGridVerticesCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGridVerticesCollection.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGridVerticesCollection.Location = new System.Drawing.Point(0, 0);
            this.propertyGridVerticesCollection.Name = "propertyGridVerticesCollection";
            this.propertyGridVerticesCollection.Size = new System.Drawing.Size(426, 322);
            this.propertyGridVerticesCollection.TabIndex = 0;
            this.propertyGridVerticesCollection.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridVerticesCollection_PropertyValueChanged);
            this.propertyGridVerticesCollection.VisibleChanged += new System.EventHandler(this.propertyGridVerticesCollection_VisibleChanged);
            // 
            // VerticesCollection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(426, 322);
            this.Controls.Add(this.propertyGridVerticesCollection);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "VerticesCollection";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "VerticesCollection";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid propertyGridVerticesCollection;
    }
}