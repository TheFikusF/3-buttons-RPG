using RPG.Entities.Serialization;
using RPG.Entities.Stats;
using RPG.Items;

namespace RPG.Entities
{
    public class Enemy : ReligiousEntity
    {
        public Enemy(string name, 
            int maxHealth, 
            Inventory inventory, 
            EntityStats stats,
            EntityActions actions,
            God god,
            int level, 
            int attack = 0,  
            float hitChance = 90,
            float critMultiplier = 2.3f) : 
            base(name, maxHealth, inventory, stats, actions, god, level, attack, hitChance, critMultiplier)
        {
        }

        public Enemy(SerializedEntity serializedEntity, int level) : base(serializedEntity, level) 
        {
            Heal(Health.MaxValue);
            AddMana(Mana.MaxValue);
        }

        public static Enemy Slime(int level) => new Enemy("Slime", 20, Inventory.Human(), new EntityStats(10, 0, 10), new EntityActions(2), EntitiesRepository.Gods[0], level, attack: 10);
    }
}
