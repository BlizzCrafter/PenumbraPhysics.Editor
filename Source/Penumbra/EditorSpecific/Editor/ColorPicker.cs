using Penumbra.Utilities.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using static Penumbra.Utilities.Control.LightShapeSelectionControl;

namespace Penumbra.Utilities.Editor
{
    public class ColorPicker : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (provider != null)
            {
                IWindowsFormsEditorService editorService = 
                    (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                
                if (editorService != null)
                {
                    LightShapeSelectionControl selectionControl =
                        new LightShapeSelectionControl(
                        (MarqueeLightShape)value,
                        editorService);

                    editorService.DropDownControl(selectionControl);

                    value = selectionControl.LightShape;
                }
            }

            return value;
        }
    }
}
