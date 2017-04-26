using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using System;
using System.Windows.Forms;

namespace PenumbraPhysics.Editor.UITypeEditors
{
    public partial class VerticesCollection : Form
    {
        public object SelectedObject { get; set; }
        public Vertices VerticesReference { get; set; }
        public PolygonShape PolygonShapeReference { get; set; }
        public PolygonShape PolygonShapeReferenceCopy { get; set; }

        public VerticesCollection()
        {
            InitializeComponent();
        }

        private void propertyGridVerticesCollection_VisibleChanged(object sender, System.EventArgs e)
        {
            if (SelectedObject != null) propertyGridVerticesCollection.SelectedObject = SelectedObject;
        }

        private void propertyGridVerticesCollection_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            int index = -1;
            if (int.TryParse(e.ChangedItem.Label.Trim('[', ']'), out index))
            {
                try
                {
                    VerticesReference[index] = (Vector2)e.ChangedItem.Value;
                    PolygonShapeReference.Vertices = VerticesReference;
                    VerticesReference = PolygonShapeReference.Vertices;
                    SelectedObject = PolygonShapeReference.Vertices.ToArray();
                    propertyGridVerticesCollection.SelectedObject = SelectedObject;
                    PolygonShapeReference.TriggerVerticesChangedEvent();
                }
                catch
                {
                    PolygonShapeReference = PolygonShapeReferenceCopy;
                    VerticesReference = PolygonShapeReference.Vertices;
                    SelectedObject = PolygonShapeReference.Vertices.ToArray();
                    propertyGridVerticesCollection.SelectedObject = SelectedObject;
                    PolygonShapeReference.TriggerVerticesChangedEvent();
                }
            }
        }
    }
}
