using FarseerPhysics.Collision.Shapes;
using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using PenumbraPhysics.Editor.UITypeEditors;
using FarseerPhysics.Common;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace FarseerPhysics.EditorSpecific.Editor
{
    public class VerticesCollectionEditor : CollectionEditor
    {
        public VerticesCollectionEditor(Type type)
            : base(type)
        { }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context == null || context.Instance == null)
                return base.GetEditStyle(context);

            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, 
            IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                IWindowsFormsEditorService editorService =
                    (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                
                if (editorService != null)
                {
                    using (VerticesCollection form = new VerticesCollection())
                    {
                        form.SelectedObject = ((Vertices)value).ToArray();
                        form.VerticesReference = (Vertices)value;
                        form.PolygonShapeReference = (PolygonShape)context.Instance;
                        form.PolygonShapeReferenceCopy = (PolygonShape)context.Instance;

                        if (editorService.ShowDialog(form) != System.Windows.Forms.DialogResult.OK)
                        {
                            
                        }
                    }
                }
                else return base.EditValue(context, provider, value);
            }
            return value;
        }
    }
}
