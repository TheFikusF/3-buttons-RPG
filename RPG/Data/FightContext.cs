using RPG.Entities;
using RPG.Items;
using static RPG.Entities.EntityActions;

namespace RPG.Data
{
    public struct FightContext
    {
        public required Entity Actor { get; init; }
        public List<Entity> Allies { get; init; }
        public Entity Target { get; init; }
        public List<Entity> Opponents { get; init; }
        public Attack Attack { get; init; }
        public Spell SpellUsed { get; init; }
        public SpellResult SpellUseResult { get; init; }
        public ItemUseResult ItemUseResult { get; init; }
    }
}
