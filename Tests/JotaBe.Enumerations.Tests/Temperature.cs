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
