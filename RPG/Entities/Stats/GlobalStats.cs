
namespace RPG.Entities.Stats
{
    public abstract class GlobalStats<T> where T : Enum
    {
        public enum StatIncrementType
        {
            Linear, Sqrt, Log, Pow
        }

        [Serializable]
        public struct Stat
        {
            private int _level = 1;
            private string _description = "Upgrades damage";

            private StatIncrementType _incrementType = StatIncrementType.Linear;
            private float _defaultValue = 10;
            private float _increment = 5;

            public int Level => _level;
            public string Description => _description;

            public float DefaultValue => _defaultValue;
            public float Increment => _increment;
            public float Value => GetValue(Level);

            public Stat(string description, float defaultValue, float increment,
                int level = 1,
                GlobalStats<T>.StatIncrementType incrementType = StatIncrementType.Linear)
            {
                _level = level;
                _description = description;
                _incrementType = incrementType;
                _defaultValue = defaultValue;
                _increment = increment;
            }

            public Stat(Stat stat, int level)
            {
                _level = level;
                _description = stat.Description;
                _incrementType = stat._incrementType;
                _defaultValue = stat._defaultValue;
                _increment = stat._increment;
            }

            public float GetValue(int level)
            {
                return _incrementType switch
                {
                    StatIncrementType.Linear => _defaultValue + _increment * (level - 1),
                    StatIncrementType.Sqrt => (float)(_defaultValue - Math.Sqrt(level - 1) / _increment),
                    StatIncrementType.Log => _defaultValue,
                    StatIncrementType.Pow => (float)(_defaultValue + Math.Pow(level, _increment)),
                    _ => _defaultValue
                };
            }
        }

        private Dictionary<T, Stat> _stats = new();
        private BuffStats<T> _buffs;
        private static readonly IEnumerable<T> _defaultStats = Enum.GetValues(typeof(T)).OfType<T>();

        public GlobalStats(Dictionary<T, Stat> stats)
        {
            _stats = stats;
            _buffs = new BuffStats<T>(this);
        }

        public GlobalStats() : this(_defaultStats.ToDictionary(x => x, x => new Stat("description", 1, 1))) { }

        public void AddLevel(T name) => _stats[name] = new Stat(_stats[name], _stats[name].Level + 1);
        public void SetLevel(T name, int level) => _stats[name] = new Stat(_stats[name], level);
        public Stat GetStat(T name) => _stats[name];
        public float GetValue(T name, int level) => GetStat(name).GetValue(level);
        public float GetValue(T name) => GetStat(name).Value;
        public float GetFullValue(T name) => _buffs.GetFullValue(name);
        public float GetFullValue(T name, int level) => _buffs.GetFullValue(name, level);

        public float GetFullValue(T name, params GlobalStats<T>[] stats)
        {
            return GetFullValue(name, GetStat(name).Level + stats.Sum(x => x.GetStat(name).Level));
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, _stats.Select(x => $"{x.Key}: {GetFullValue(x.Key)}"));
        }
    }
}