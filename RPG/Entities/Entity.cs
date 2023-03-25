using System.Linq;
using RPG.Entities.Stats;
using RPG.Items;
using RPG.Utils;

namespace RPG.Entities
{
    public abstract class Entity
    {
        private string _name;
        private int _level;
        private int _experience;

        private int _health;
        private int _maxHealth;

        private int _defence;
        private int _attack;

        private float _hitChance;
        private float _critMultiplier;

        private int _money;

        private Inventory _inventory;
        private EntityStats _entitryStats;

        public string Name => _name;
        public int Level => _level;
        public int Experience => _experience;
        public int MaxExperience => _level * Consts.ExperiencePerLvl;

        public int Money => _money;

        public int Health
        {
            get => _health;
            set
            {
                _health = value < 0 ? 0 : value;
                _health = _health > MaxHealth ? MaxHealth : _health;
            }
        }
        public int MaxHealth => _maxHealth + Inventory.Health + (int)(Stats.GetFullValue(StatType.Strength) * 3);
        public int Defence => _defence + Inventory.Defence + (int)(Stats.GetFullValue(StatType.Strength) * 0.2);
        public int Attack => _attack + Inventory.Attack + (int)Stats.GetFullValue(StatType.Strength);
        
        public Inventory Inventory => _inventory;
        public EntityStats Stats => _entitryStats;

        public float Evasion => MathF.Pow(MathF.Log10(Stats.GetFullValue(StatType.Agility)), 1.666f) * 10; 
        public float HitChance => _hitChance;
        public float CritChance => MathF.Pow(MathF.Log10(Extensions.Lerp(Stats.GetFullValue(StatType.Agility), Stats.GetFullValue(StatType.Inteligence), 0.7f)), 2.966f) * 10;
        public float CritMultiplier => _critMultiplier;

        public Entity(string name, int maxHealth, 
            Inventory inventory, 
            EntityStats stats,
            int level = 1, 
            int attack = 10, 
            float hitChance = 90,  
            float critMultiplier = 2.3f)
        {
            _inventory = inventory;
            _entitryStats = stats;
            _name = name;
            _level = level;
            
            _maxHealth = maxHealth;
            _health = MaxHealth;
            
            _experience = 0;
            _money = 0;
            
            _attack = attack;
            
            _hitChance = hitChance;
            _critMultiplier = critMultiplier;
        }

        public bool AddExperience(int amount)
        {
            _experience += amount;

            if (_experience >= _level * Consts.ExperiencePerLvl)
            {
                _level++;
                _experience = 0;
                return true;
            }

            return false;
        }

        public bool TryTakeDamage(int amount)
        {
            Health -= amount - Defence;
            return Health <= 0;
        }

        public void Heal(int amount)
        {
            Health += amount;
        }

        public override string ToString()
        {
            var str = $"|| Entity: {Name}" + Environment.NewLine +
                $"LVL {Level}: {Experience}/{MaxExperience}" + Environment.NewLine + Environment.NewLine +
                $"Health: {Health}/{MaxHealth}" + Environment.NewLine +
                $"Defence: {Defence}" + Environment.NewLine +
                $"Attack: {Attack}" + Environment.NewLine + Environment.NewLine +
                Stats + Environment.NewLine + Environment.NewLine +
                $"Evasion: {Evasion}" + Environment.NewLine +
                $"Hit chance: {HitChance}" + Environment.NewLine +
                $"Crit: {CritMultiplier}x for {CritChance}%";

            return str;
        }

        public string ToShortString()
        {
            var str = $"|| Entity: {Name}{$"LVL {Level}".PadLeft(23 - Name.Length)}" + Environment.NewLine +
                $"Health: {Health}/{MaxHealth}";

            return str;
        }
    }
}
