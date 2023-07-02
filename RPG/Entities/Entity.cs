using RPG.Data;
using RPG.Entities.Serialization;
using RPG.Entities.Stats;
using RPG.Items;
using RPG.Items.Serialization;
using RPG.Utils;
using System.Text;

namespace RPG.Entities
{
    public abstract class Entity
    {
        private string _name;
        private int _level;

        private Bar _experience;
        private Bar _health;
        private Bar _mana;

        private int _defence;
        private int _attack;

        private float _hitChance;
        private float _critMultiplier;

        private int _money;
        private int _levelUpPoints;

        private Inventory _inventory;
        private EntityStats _entityStats;
        private EntityActions _entityActions;

        private int _lastDamageTook;

        public string Name => _name;
        public int Level => _level;
        public ReadonlyBar Experience => new ReadonlyBar(_experience);

        public int Money => _money;
        public int LevelUpPoints => _levelUpPoints;

        public ReadonlyBar Health => new ReadonlyBar(_health);
        public ReadonlyBar Mana => new ReadonlyBar(_mana);

        public int Defence => _defence + Inventory.Defence + (int)(Stats.GetFullValue(StatType.Strength) * 0.2);
        public virtual int Attack => _attack + Inventory.Attack + (int)Stats.GetFullValue(StatType.Strength);
        
        public virtual Inventory Inventory => _inventory;
        public virtual EntityStats Stats => _entityStats;
        public virtual EntityActions Actions => _entityActions;

        public float Evasion => MathF.Pow(MathF.Log10(Stats.GetFullValue(StatType.Agility)), 1.666f) * 10; 
        public float HitChance => _hitChance;
        public float CritChance => MathF.Pow(MathF.Log10(Extensions.Lerp(Stats.GetFullValue(StatType.Agility), Stats.GetFullValue(StatType.Intelligence), 0.7f)), 2.966f) * 10;
        public float CritMultiplier => _critMultiplier;

        public int LastDamageTook => _lastDamageTook;

        public Entity(string name, int maxHealth, 
            Inventory inventory, 
            EntityStats stats,
            EntityActions actions,
            int level = 1, 
            int attack = 10, 
            float hitChance = 90,  
            float critMultiplier = 2.3f)
        {
            _inventory = inventory;
            _entityStats = stats;
            _entityActions = actions;

            _name = name;
            _level = level;

            _health = new Bar(0, maxHealth, maxHealth, getMaxValue: (x) => x + Inventory.Health + (int)(Stats.GetFullValue(StatType.Strength) * 3));
            _mana = new Bar(0, 30, 30, getMaxValue: (x) => x + (int)(Stats.GetFullValue(StatType.Intelligence) * 3));
            _experience = new Bar(0, Consts.ExperiencePerLvl, 0, getMaxValue: (x) => _level * Consts.ExperiencePerLvl);
            
            _money = 0;
            
            _attack = attack;
            
            _hitChance = hitChance;
            _critMultiplier = critMultiplier;

            _lastDamageTook = 0;

            AllocatePoints();
        }

        public Entity(SerializedEntity serializedEntity, int level, int actionsSlots = 1) : 
            this(serializedEntity.Name, 
                serializedEntity.MaxHealth, 
                new Inventory(serializedEntity.InventorySlots), 
                EntityStats.FromSerialized(serializedEntity.Stats),
                new EntityActions(actionsSlots),
                level,
                serializedEntity.Attack,
                serializedEntity.HitChance,
                serializedEntity.CritMultiplier)
        { 
            foreach(string itemName in serializedEntity.EquipedItems)
            {
                if(ItemsRepository.TryGetItem(itemName, out InventoryItem item))
                {
                    Inventory.Equip(new InventoryItem(item));
                }
            }

            foreach (string itemName in serializedEntity.Inventory)
            {
                if (ItemsRepository.TryGetItem(itemName, out InventoryItem item))
                {
                    Inventory.AddToInventory(new InventoryItem(item));
                }
            }
        }

        public bool AddExperience(int amount)
        {
            _experience.Value += amount;

            if (_experience.Value >= _experience.MaxValue)
            {
                _level++;
                _levelUpPoints++;
                _experience.Value = 0;
                return true;
            }

            return false;
        }

        public void AddMoney(int amount) => _money += amount;

        public bool TryAddStatLevel(StatType stat)
        {
            if(_levelUpPoints <= 0)
            {
                return false;
            }

            _entityStats.AddLevel(stat);
            Heal(Health.MaxValue);
            AddMana(Mana.MaxValue);
            _levelUpPoints--;

            return true;
        }

        public bool TryTakeMoney(int amount)
        {
            if(_money - amount < 0)
            {
                return false;
            }

            _money -= amount;
            return true;
        }

        public virtual bool TryTakeDamage(int amount)
        {
            var initialHealth = _health.Value;
            _health.Value -= amount - Defence;
            _lastDamageTook = initialHealth - _health.Value;

            return _health.Value <= 0;
        }

        public virtual void Heal(int amount)
        {
            _health.Value += amount;
        }

        public bool TryTakeMana(int amount)
        {
            if(_mana.Value - amount < 0)
            {
                return false;
            }

            _mana.Value -= amount;
            return true;
        }

        public void AddMana(int amount)
        {
            _mana.Value += amount;
        }

        public EntityActions.SpellResult CastSpell(int slot, List<Entity> entities) => _entityActions.CastSpell(slot, this, entities);

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"|| Entity: {Name}");
            builder.AppendLine($"LVL {Level}: {Experience.Value}/{Experience.MaxValue}{Environment.NewLine}");
            builder.AppendLine($"Health: {Health.Value}/{Health.MaxValue}");
            builder.AppendLine($"Mana: {Mana.Value}/{Mana.MaxValue}");
            builder.AppendLine($"Defence: {Defence}");
            builder.AppendLine($"Attack: {Attack}{Environment.NewLine}");
            builder.AppendLine($"{Stats}{Environment.NewLine}");
            builder.AppendLine($"Evasion chance: {Evasion:0.##}%");
            builder.AppendLine($"Hit chance: {HitChance:0.##}%");
            builder.AppendLine($"Crit: {CritMultiplier:0.##}x for {CritChance:0.##}%");

            return builder.ToString();
        }

        public string ToShortString()
        {
            var str = $"|> Entity: {Name}{$"LVL {Level}".PadLeft(23 - Name.Length)}" + Environment.NewLine +
                $"| Health: {Health.Value}/{Health.MaxValue}";

            return str;
        }

        private void AllocatePoints()
        {
            List<StatType> currentStats = EntityStats.DefaultStats.ToList();
            int total = currentStats.Sum(x => (int)Stats.GetValue(x));
            
            Random rand = new Random();
            for (int i = 0; i < Level - 1; i++)
            {
                int roll = rand.Next(1, total + 1);

                AllocatePoint(currentStats, roll);
                total++;
            }
        }

        private void AllocatePoint(List<StatType> currentStats, int roll)
        {
            for (int j = 0; j < currentStats.Count; j++)
            {
                int statValue = (int)Stats.GetValue(currentStats[j]);
                if (roll <= statValue)
                {
                    Stats.AddLevel(currentStats[j]);
                    break;
                }

                roll -= statValue;
            }
        }
    }
}
