using System;
using System.Windows.Forms;

namespace PenumbraPhysics.Editor.UITypeEditors
{
    public partial class FixtureListCollection : Form
    {
        public object SelectedObject { get; set; }

        public FixtureListCollection()
        {
            InitializeComponent();
        }

        private void propertyGridFixtureList_VisibleChanged(object sender, EventArgs e)
        {
            if (SelectedObject != null)
            {
                propertyGridFixtureList.SelectedObject = SelectedObject;
                propertyGridFixtureList.ExpandAllGridItems();
            }
        }
    }
}
