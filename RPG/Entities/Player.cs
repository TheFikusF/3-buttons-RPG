using RPG.Entities.Stats;
using RPG.Items;

namespace RPG.Entities
{
    public class Player : Entity
    {
        public Player(string name, int maxHealth) : base(name, maxHealth, Inventory.Human(), new EntityStats(10, 10, 10))
        {

        }
    }
}
