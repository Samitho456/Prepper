using System;
using System.Collections.Generic;
using System.Text;

namespace Prepper
{
    public class UnitEnum
    {
        public enum Unit
        {
            Gram,
            Kilogram,
            Milliliter,
            Liter,
            Piece,
            Teaspoon,
            Tablespoon,
            Cup,
            Ounce,
            Pound
        }

        public static bool ConvertUnit(Unit fromUnit, Unit toUnit, float amount, out float convertedAmount)
        {
            // Conversion logic can be implemented here
            // For simplicity, let's assume 1:1 conversion for now
            if(fromUnit == Unit.Gram && toUnit == Unit.Kilogram)
            {
                convertedAmount = amount / 1000f;
                return true;
            }
            else if(fromUnit == Unit.Kilogram && toUnit == Unit.Gram)
            {
                convertedAmount = amount * 1000f;
                return true;
            }

            convertedAmount = amount;
            return true;
        }
    }
}
