using RPG.Items;

namespace RPG.Entities
{
    public class Player : Entity
    {
        public Player(string name, int maxHealth) : base(name, maxHealth, Inventory.Human())
        {

        }
    }
}
