using RPG.Items;

namespace RPG.Entities
{
    public class Enemy : Entity
    {
        public Enemy(string name, 
            int maxHealth, 
            Inventory inventory, 
            int level, 
            int attack = 10, 
            float evasion = 10, 
            float hitChance = 90,
            float critChance = 10,
            float critMultiplier = 2.3f) : 
            base(name, maxHealth, inventory, level, attack, evasion, hitChance, critChance, critMultiplier)
        {
        }

        public static Enemy Slime(int level) => new Enemy("Slime", 20, Inventory.Human(), level, evasion: 20);
    }
}
