using RPG.Entities.Serialization;
using RPG.Entities.Stats;
using RPG.Items;

namespace RPG.Entities
{
    public class God : Entity
    {
        public static readonly God Odin = new God("Odin", 300, Inventory.Human(), new EntityStats(40, 50, 60), new EntityActions(2), 1, 20, 100, description: "Wise Odin");

        private string _description = "";

        public EntityStats GiftedStats => new EntityStats((int)(Stats.GetStat(StatType.Agility).Level * 0.1f),
            (int)(Stats.GetStat(StatType.Strength).Level * 0.1f),
            (int)(Stats.GetStat(StatType.Intelligence).Level * 0.1f));

        public God(string name,
            int maxHealth,
            Inventory inventory,
            EntityStats stats,
            EntityActions actions,
            int level,
            int attack = 0,
            float hitChance = 90,
            float critMultiplier = 2.3f,
            string description = "") :
            base(name, maxHealth, inventory, stats, actions, level, attack, hitChance, critMultiplier)
        {
            _description = description;
        }

        public God(SerializedEntity serializedEntity, int level) : base(serializedEntity, level) { }
    }
}
