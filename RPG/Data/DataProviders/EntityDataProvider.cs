using RPG.Entities;
using RPG.Entities.Serialization;
using RPG.Entities.Stats;
using RPG.Items;
using static RPG.Entities.Stats.StatType;
using static RPG.Items.SlotType;

namespace RPG.Data.DataProviders
{
    public class EntityDataProvider : IEntityDataProvider
    {
        private Dictionary<string, List<SerializedEntity>> _enemies = new();
        private List<God> _gods = new();

        public Dictionary<string, List<SerializedEntity>> Enemies => _enemies.ToDictionary(x => x.Key, x => x.Value);
        public List<God> Gods => _gods.ToList();

        public EntityDataProvider()
        {
            _enemies["Default"] = new List<SerializedEntity>()
            {
                new SerializedEntity("Slime", 10, new[]{ Arms }, 
                    new() { [Agility] = 10, [Strength] = 10, [Intelligence] = 10}, 10, 90, 2.3f, new(), new() ),
                new SerializedEntity("Skeleton", 10, Inventory.HumanSlots().ToArray(), 
                    new() { [Agility] = 10, [Strength] = 10, [Intelligence] = 10}, 10, 90, 2.3f, new(), new() ),
                new SerializedEntity("Goblin", 20, Inventory.HumanSlots().ToArray(), 
                    new() { [Agility] = 10, [Strength] = 10, [Intelligence] = 10}, 10, 90, 2.3f, new(), new() ),
            };

            _gods = new List<God> { new God("Odin", 300,
                Inventory.Human(),
                new EntityStats(40, 50, 60),
                new EntityActions(2, new List<EntityActions.Spell>() { EntityActions.Spell.ThunderStrike() }),
                1, 20, 100, description: "Wise Odin")
            };
        }
    }
}
