using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JotaBe.Enumerations.Tests
{
    public class Temperature
    {
        public Temperature(double value, TemperatureUnit unit)
        {
            Value = value;
            Unit = unit;
        }

        public double Value { get; set; }
        public TemperatureUnit Unit { get; set; }
    }
}
