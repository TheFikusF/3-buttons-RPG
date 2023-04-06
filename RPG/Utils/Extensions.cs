using RandN;

namespace RPG.Utils
{
    public static class Extensions
    {
        private static SmallRng _rng = SmallRng.Create();
        public static SmallRng RNG => _rng;

        public static T PickRandom<T>(this List<T> list) => list[new Random().Next(0, list.Count)];

        public static Dictionary<TKey, TValue> ToDict<TKey, TValue>(this List<TKey> dict) where TValue : class
        {
            var newdict = new Dictionary<TKey, TValue>();

            dict.ForEach(x => newdict.Add(x, null));

            return newdict;
        }

        public static float Lerp(float a, float b, float t)
        {
            return a * (1 - t) + b * t;
        }
    }
}
