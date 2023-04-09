using RPG.Entities.Serialization;
using RPG.Entities.Stats;
using RPG.Items;

namespace RPG.Entities
{
    public class ReligiousEntity : Entity
    {
        private God _god;
        public God God => _god;

        public override EntityStats Stats => God == null ? base.Stats : base.Stats.Append(God.GiftedStats);

        public ReligiousEntity(string name, 
            int maxHealth, 
            Inventory inventory, 
            EntityStats stats,
            God god,
            int level = 1, 
            int attack = 10, 
            float hitChance = 90, 
            float critMultiplier = 2.3F) 
            : base(name, maxHealth, inventory, stats, level, attack, hitChance, critMultiplier)
        {
            _god = god;
            Heal(MaxHealth);
        }

        public ReligiousEntity(SerializedEntity serializedEntity, int level) : base(serializedEntity, level) 
        {
            _god = God.Odin;
            Heal(MaxHealth);
        }

        public override string ToString()
        {
            return base.ToString() + Environment.NewLine + $"God: {God.Name}";
        }
    }
}
