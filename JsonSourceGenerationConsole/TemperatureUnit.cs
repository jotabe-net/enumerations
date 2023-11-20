using JotaBe.Enumerations;
using JotaBe.Enumerations.Json;
using System.Text.Json.Serialization;

namespace JsonSourceGenerationConsole
{
    public class DecimalMultiplier : EnumerationClass<DecimalMultiplier, char>
    {
        protected DecimalMultiplier(string name, char value, long factor)
            : base(name, value)
        {
            Factor = factor;
        }

        public long Factor { get; private set; }

        public double ConvertTo(float value, DecimalMultiplier newMultiplier)
        {
            var newValue = value * Factor / (double)newMultiplier.Factor;
            return newValue;
        }

        public static readonly DecimalMultiplier Kilo = new DecimalMultiplier(nameof(Kilo), 'K', 1_000);
        public static readonly DecimalMultiplier Mega = new DecimalMultiplier(nameof(Mega), 'M', 1_000_000);
        public static readonly DecimalMultiplier Giga = new DecimalMultiplier(nameof(Giga), 'G', 1_000_000_000);
        public static readonly DecimalMultiplier Tera = new DecimalMultiplier(nameof(Tera), 'T', 1_000_000_000_000);
    }

    [JsonSourceGenerationOptions(
        Converters = [typeof(EnumerationClassValueConverter)],
        PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower
    )]
    [JsonSerializable(typeof(MultipliedValue))]
    internal partial class SourceGenerationContext : JsonSerializerContext
    {

    }

    public class MultipliedValue
    {
        public MultipliedValue(double value, DecimalMultiplier multiplier)
        {
            BaseValue = value;
            Multiplier = multiplier;
        }

        public double BaseValue { get; set; }
        public DecimalMultiplier Multiplier { get; set; }
    }


}
