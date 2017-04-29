using FarseerPhysics;
using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Penumbra.EditorSpecific.Converter
{
    public class PhysicsPositionConverter : DoubleConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            Vector2 input = (Vector2)value;
            Vector2 output = ConvertUnits.ToDisplayUnits(input);

            return output.ToString();
        }
        
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string input = value.ToString();
            string[] parse1 = input.Split(':');

            float xValue = 0, yValue = 0;
            float.TryParse(parse1[1].TrimEnd('Y', ' '), out xValue);
            float.TryParse(parse1[2].TrimEnd('}'), out yValue);
            
            return new Vector2(ConvertUnits.ToSimUnits(xValue), ConvertUnits.ToSimUnits(yValue));
        }
    }
}
