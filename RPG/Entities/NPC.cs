using RPG.Entities.NPCSpecializations;
using RPG.Entities.Stats;
using RPG.Items;

namespace RPG.Entities
{
    public class NPC<T> : ReligiousEntity where T : NPCSpecializtion, new()
    {
        private readonly T _specialization;

        public T Specialization => _specialization;

        public NPC(string name, 
            int maxHealth, 
            Inventory inventory, 
            EntityStats stats, 
            EntityActions actions, 
            God god, 
            int level = 1, 
            int attack = 10, 
            float hitChance = 90, 
            float critMultiplier = 2.3F) : 
            base(name, maxHealth, inventory, stats, actions, god, level, attack, hitChance, critMultiplier)
        {
            _specialization = new();
        }
    }
}
