﻿using RPG.Entities.Serialization;
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
        private int _experience;

        private int _health;
        private int _maxHealth;

        private int _defence;
        private int _attack;

        private float _hitChance;
        private float _critMultiplier;

        private int _money;
        private int _levelUpPoints;

        private Inventory _inventory;
        private EntityStats _entityStats;

        public string Name => _name;
        public int Level => _level;
        public int Experience => _experience;
        public int MaxExperience => _level * Consts.ExperiencePerLvl;

        public int Money => _money;
        public int LevelUpPoints => _levelUpPoints;

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
        public virtual EntityStats Stats => _entityStats;

        public float Evasion => MathF.Pow(MathF.Log10(Stats.GetFullValue(StatType.Agility)), 1.666f) * 10; 
        public float HitChance => _hitChance;
        public float CritChance => MathF.Pow(MathF.Log10(Extensions.Lerp(Stats.GetFullValue(StatType.Agility), Stats.GetFullValue(StatType.Intelligence), 0.7f)), 2.966f) * 10;
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
            _entityStats = stats;
            _name = name;
            _level = level;
            
            _maxHealth = maxHealth;
            _health = maxHealth;
            
            _experience = 0;
            _money = 0;
            
            _attack = attack;
            
            _hitChance = hitChance;
            _critMultiplier = critMultiplier;
        }

        public Entity(SerializedEntity serializedEntity, int level) : 
            this(serializedEntity.Name, 
                serializedEntity.MaxHealth, 
                new Inventory(serializedEntity.InventorySlots), 
                EntityStats.FromSerialized(serializedEntity.Stats),
                level,
                serializedEntity.Attack,
                serializedEntity.HitChance,
                serializedEntity.CritMultiplier)
        { 
            foreach(string itemName in serializedEntity.EquipedItems)
            {
                if(ItemsRepository.TryGetItem(itemName, out Item item))
                {
                    Inventory.Equip(new Item(item));
                }
            }

            foreach (string itemName in serializedEntity.Inventory)
            {
                if (ItemsRepository.TryGetItem(itemName, out Item item))
                {
                    Inventory.AddToInventory(new Item(item));
                }
            }
        }

        public bool AddExperience(int amount)
        {
            _experience += amount;

            if (_experience >= _level * Consts.ExperiencePerLvl)
            {
                _level++;
                _levelUpPoints++;
                _experience = 0;
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
            _levelUpPoints--;

            return true;
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
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"|| Entity: {Name}");
            builder.AppendLine($"LVL {Level}: {Experience}/{MaxExperience}{Environment.NewLine}");
            builder.AppendLine($"Health: {Health}/{MaxHealth}");
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
            var str = $"|| Entity: {Name}{$"LVL {Level}".PadLeft(23 - Name.Length)}" + Environment.NewLine +
                $"Health: {Health}/{MaxHealth}";

            return str;
        }
    }
}
