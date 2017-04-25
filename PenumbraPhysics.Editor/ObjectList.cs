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

        public ObjectList()
        {
            InitializeComponent();
        }

        private void propertyGridObjectDetails_VisibleChanged(object sender, EventArgs e)
        {
            propertyGridObjectDetails.SelectedObjects = SelectedObjects;
        }
    }
}
