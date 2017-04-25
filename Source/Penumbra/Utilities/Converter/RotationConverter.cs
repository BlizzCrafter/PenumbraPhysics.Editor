using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Penumbra
{
    public class RotationConverter : DoubleConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return false;
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            float sum = MathHelper.ToDegrees(float.Parse(value.ToString()));
            return Math.Round(sum, 0, MidpointRounding.AwayFromZero).ToString();
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            float sum = MathHelper.ToRadians((float)Math.Round(float.Parse(value.ToString()), 0, MidpointRounding.AwayFromZero));
            return sum;
        }
    }
}
