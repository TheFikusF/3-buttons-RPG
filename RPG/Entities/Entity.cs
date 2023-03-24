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

        private float _evasion;
        private float _hitChance;
        private float _critChance;
        private float _critMultiplier;

        private int _money;

        private Inventory _inventory;

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
        public int MaxHealth => _maxHealth + Inventory.Health;
        public int Defence => _defence + Inventory.Defence;
        public int Attack => _attack + Inventory.Attack;
        
        public Inventory Inventory => _inventory;

        public float Evasion => _evasion; 
        public float HitChance => _hitChance;
        public float CritChance => _critChance;
        public float CritMultiplier => _critMultiplier;

        public Entity(string name, int maxHealth, 
            Inventory inventory, 
            int level = 1, 
            int attack = 10, 
            float evasion = 10, 
            float hitChance = 90, 
            float critChance = 10, 
            float critMultiplier = 2.3f)
        {
            _inventory = inventory;
            _name = name;
            _level = level;
            
            _maxHealth = maxHealth;
            _health = maxHealth;
            
            _experience = 0;
            _money = 0;
            
            _attack = attack;
            
            _evasion = evasion;
            _hitChance = hitChance;
            _critChance = critChance;
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
                $"Attack: {Attack}" + Environment.NewLine;

            return str;
        }

        public string ToShortString()
        {
            var str = $"|| Entity: {Name}{$"LVL {Level}".PadLeft(23 - Name.Length)}" + Environment.NewLine +
                //$"LVL {Level}: {Experience}/{MaxExperience}" + Environment.NewLine +
                $"Health: {Health}/{MaxHealth}";

            return str;
        }
    }
}
