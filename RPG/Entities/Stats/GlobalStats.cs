
namespace RPG.Entities.Stats
{
    public abstract class GlobalStats<T> where T : Enum
    {
        public enum StatIncrementType
        {
            Linear, Sqrt, Log, Pow
        }

        [Serializable]
        public class Stat
        {
            private T _name;
            private int _level = 1;
            private string _description = "Upgrades damage";

            private StatIncrementType _incrementType = StatIncrementType.Linear;
            private float _defaultValue = 10;
            private float _increment = 5;

            public T Name => _name;
            public int Level => _level;
            public string Description => _description;

            public float DefaultValue => _defaultValue;
            public float Increment => _increment;
            public float Value => _incrementType switch
            {
                StatIncrementType.Linear => _defaultValue + _increment * (_level - 1),
                StatIncrementType.Sqrt => (float)(_defaultValue - Math.Sqrt(_level - 1) / _increment),
                StatIncrementType.Log => _defaultValue,
                StatIncrementType.Pow => (float)(_defaultValue + Math.Pow(Level, _increment)),
                _ => _defaultValue
            };

            public Stat(T name, string description, float defaultValue, float increment,
                int level = 1,
                GlobalStats<T>.StatIncrementType incrementType = StatIncrementType.Linear)
            {
                _name = name;
                _level = level;
                _description = description;
                _incrementType = incrementType;
                _defaultValue = defaultValue;
                _increment = increment;
            }

            public void AddLevel()
            {
                SetLevel(_level + 1);
            }

            public void SetLevel(int level)
            {
                _level = level;
            }
        }

        private List<Stat> _stats = new();
        private BuffStats<T> _buffs;

        public List<Stat> Stats => _stats.ToList();

        public GlobalStats(List<Stat> stats)
        {
            _stats = stats;
            _buffs = new BuffStats<T>(this);
        }

        public GlobalStats() : this(Enum.GetValues(typeof(T)).OfType<T>().Select(x => new Stat(x, "description", 1, 1)).ToList()) { }

        public void AddLevel(T name) => _stats.First(x => x.Name.Equals(name)).AddLevel();
        public void SetLevel(T name, int level) => _stats.First(x => x.Name.Equals(name)).SetLevel(level);
        public Stat GetStat(T name) => _stats.First(x => x.Name.Equals(name));
        public float GetValue(T name) => GetStat(name).Value;
        public float GetFullValue(T name) => _buffs.GetFullValue(name);

        public override string ToString()
        {
            return string.Join(Environment.NewLine, _stats.Select(x => $"{x.Name}: {GetFullValue(x.Name)}"));
        }
    }
}