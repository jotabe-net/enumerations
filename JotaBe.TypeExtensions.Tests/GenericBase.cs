namespace JotaBe.TypeExtensions.Tests
{
    public class GenericBase<TKey, TValue>
        where TKey : notnull
    {
        public GenericBase()
        {
            TheDictionary = new Dictionary<TKey, TValue>();
        }

        public Dictionary<TKey, TValue> TheDictionary { get; set; }
    }


}