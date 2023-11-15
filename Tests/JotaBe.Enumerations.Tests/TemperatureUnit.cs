namespace JotaBe.Enumerations.Tests
{
    public class TemperatureUnit : EnumerationClass<TemperatureUnit, char>
    {
        // TODO: keep this protected or private !! can't be allowed as public
        protected TemperatureUnit(string name, char value) : base(name, value)
        {
        }

        // TODO: Define elements as readonly static

        public readonly static TemperatureUnit Celsius = new TemperatureUnit(nameof(Celsius), 'C');
        public readonly static TemperatureUnit Kelvin = new TemperatureUnit(nameof(Kelvin), 'K');
        public readonly static TemperatureUnit Fahrenheit = new TemperatureUnit(nameof(Fahrenheit), 'F');
    }


}