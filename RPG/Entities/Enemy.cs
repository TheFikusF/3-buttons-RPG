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
            AllocatePoints();
            int i = 1;
            Heal(Health.MaxValue);
            AddMana(Mana.MaxValue);
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
                    DefaultStats.AddLevel(currentStats[j]);
                    break;
                }

                roll -= statValue;
            }
        }

        public static Enemy Slime(int level) => new Enemy("Slime", 20, Inventory.Human(), new EntityStats(10, 0, 10), new EntityActions(2), EntitiesRepository.Gods[0], level, attack: 10);
    }
}
