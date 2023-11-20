namespace JotaBe.Enumerations.Tests
{
    public class TemperatureUnit : EnumerationClass<TemperatureUnit, char>
    {
        protected TemperatureUnit(string name, char value) : base(name, value)
        {
        }

        public readonly static TemperatureUnit Celsius = new TemperatureUnit(nameof(Celsius), 'C');
        public readonly static TemperatureUnit Kelvin = new TemperatureUnit(nameof(Kelvin), 'K');
        public readonly static TemperatureUnit Fahrenheit = new TemperatureUnit(nameof(Fahrenheit), 'F');
    }

}