using RandN;
using RandN.Compat;
using RPG.Entities;

namespace RPG.Utils
{
    public static class Extensions
    {
        public delegate TResultType FightAction<TResultType>(Entity user, List<Entity> opponents);
        public delegate TResultType FightAction<TResultType, T1>(Entity user, List<Entity> opponents, T1 param1);
        public delegate TResultType FightAction<TResultType, T1, T2>(Entity user, List<Entity> opponents, T1 param1, T2 param2);
        public delegate TResultType FightAction<TResultType, T1, T2, T3>(Entity user, List<Entity> opponents, T1 param1, T2 param2, T3 param3);

        private static readonly SmallRng _rng;
        private static readonly RandomShim<SmallRng> _randomGenerator;
        public static SmallRng RNG => _rng;

        static Extensions()
        {
            _rng = SmallRng.Create();
            _randomGenerator = RandomShim.Create(RNG);
        }

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

        public static int GetRandom(int max) => GetRandom(max, 0);
        public static int GetRandom(int min, int max) => _randomGenerator.Next(min, max);
    }
}
