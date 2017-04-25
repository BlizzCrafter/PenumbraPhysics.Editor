using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PenumbraPhysics.Editor
{
    public partial class ObjectList : Form
    {
        public object[] SelectedObjects;
        public object SelectedObject;

        public ObjectList()
        {
            InitializeComponent();
        }

        private void propertyGridObjectDetails_VisibleChanged(object sender, EventArgs e)
        {
            if (SelectedObjects != null) propertyGridObjectDetails.SelectedObjects = SelectedObjects;
            else if (SelectedObject != null) propertyGridObjectDetails.SelectedObject = SelectedObject;
        }
    }
}
