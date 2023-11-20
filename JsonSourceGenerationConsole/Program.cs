using System.Text.Json;

namespace JsonSourceGenerationConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var val = new MultipliedValue(23.78, DecimalMultiplier.Mega);
            var json = JsonSerializer.Serialize(val, SourceGenerationContext.Default.MultipliedValue);
            Console.WriteLine(json);
        }
    }
}
