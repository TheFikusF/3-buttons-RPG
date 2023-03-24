namespace RPG.Utils
{
    public static class Extensions
    {
        public static Dictionary<TKey, TValue> ToDict<TKey, TValue>(this List<TKey> dict) where TValue : class
        {
            var newdict = new Dictionary<TKey, TValue>();

            dict.ForEach(x => newdict.Add(x, null));

            return newdict;
        }
    }
}
