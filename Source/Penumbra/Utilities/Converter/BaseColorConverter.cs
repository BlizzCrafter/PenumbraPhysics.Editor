using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Penumbra
{
    public class BaseColorConverter : ColorConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Color Color = Color.FromArgb(
                ((Microsoft.Xna.Framework.Color)value).A,
                ((Microsoft.Xna.Framework.Color)value).R, 
                ((Microsoft.Xna.Framework.Color)value).G, 
                ((Microsoft.Xna.Framework.Color)value).B);
            return $"#{Color.Name.Substring(2)}";
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            Color clr = ColorTranslator.FromHtml(value.ToString());
            Microsoft.Xna.Framework.Color rColor = new Microsoft.Xna.Framework.Color(clr.R, clr.G, clr.B, clr.A);
            return rColor;
        }
    }
}
