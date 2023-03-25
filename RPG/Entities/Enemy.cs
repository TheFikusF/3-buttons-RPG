using RPG.Entities.Stats;
using RPG.Items;

namespace RPG.Entities
{
    public class Enemy : Entity
    {
        public Enemy(string name, 
            int maxHealth, 
            Inventory inventory, 
            EntityStats stats,
            int level, 
            int attack = 0,  
            float hitChance = 90,
            float critMultiplier = 2.3f) : 
            base(name, maxHealth, inventory, stats, level, attack, hitChance, critMultiplier)
        {
        }

        public static Enemy Slime(int level) => new Enemy("Slime", 20, Inventory.Human(), new EntityStats(10, 0, 10), level, attack: 10);
    }
}
