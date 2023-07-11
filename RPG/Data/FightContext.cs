using RPG.Entities;
using RPG.Items;
using static RPG.Entities.EntityActions;

namespace RPG.Data
{
    public struct FightContext
    {
        public required Entity Actor { get; set; }
        public List<Entity> Allies { get; set; }
        public Entity Target { get; set; }
        public List<Entity> Opponents { get; set; }
        public Attack Attack { get; set; }
        public Spell SpellUsed { get; set; }
        public SpellResult SpellUseResult { get; set; }
        public ItemUseResult ItemUseResult { get; set; }
    }
}
