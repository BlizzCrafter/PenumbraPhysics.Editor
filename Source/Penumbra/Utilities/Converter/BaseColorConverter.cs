using System.ComponentModel;
using System.Drawing;

namespace Penumbra
{
    public class BaseColorConverter : ColorConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
