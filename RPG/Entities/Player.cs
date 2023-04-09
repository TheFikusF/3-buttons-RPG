using RPG.Entities.Serialization;
using RPG.Entities.Stats;
using RPG.Items;

namespace RPG.Entities
{
    public class Player : ReligiousEntity
    {
        public Player(string name, int maxHealth) : base(name, maxHealth, Inventory.Human(), new EntityStats(10, 10, 10), God.Odin)
        {

        }

        public Player(SerializedEntity serializedEntity, int level) : base(serializedEntity, level) { }
    }
}
