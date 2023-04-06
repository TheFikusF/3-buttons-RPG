
namespace RPG.Entities.Stats
{
    public class BuffStats<T> where T : Enum
    {
        public enum BuffType
        {
            Multiplier, Add
        }

        public class Buff
        {
            public T Name { get; private set; }
            public BuffType Type { get; private set; } = BuffType.Multiplier;
            public float Value { get; private set; } = 2f;
            public int RemoveTime { get; private set; } = 30;
        }

        private GlobalStats<T> _stats;
        private List<Buff> _buffs = new();

        public List<Buff> Buffs => _buffs.ToList();

        public BuffStats(GlobalStats<T> stats)
        {
            _stats = stats;
        }

        public void AddBuff(Buff buff, bool useRemoveTime)
        {
            if (useRemoveTime)
            {
                AddBuff(buff, buff.RemoveTime);
                return;
            }

            AddBuff(buff);
        }

        public void AddBuff(Buff buff, int turns)
        {

        }

        public void AddBuff(Buff buff)
        {
            _buffs.Add(buff);
        }

        public void RemoveBuff(Buff buff)
        {
            _buffs.Remove(buff);
        }

        public float GetFullValue(T name, int level)
        {
            var value = _stats.GetValue(name, level);

            foreach (var buff in _buffs.Where(x => x.Name.Equals(name)))
            {
                switch (buff.Type)
                {
                    case BuffType.Multiplier: value *= buff.Value; break;
                    case BuffType.Add: value += buff.Value; break;
                    default: value += buff.Value; break;
                }
            }

            return value;
        }

        public float GetFullValue(T name)
        {
            return GetFullValue(name, _stats.GetStat(name).Level);
        }
    }
}