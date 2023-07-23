using RPG.Entities;

namespace RPG.Fight
{
    public class TurnBasedFight : Fight
    {
        private int _turn;

        public TurnBasedFight(Entity actor, List<Entity> allies, List<Entity> opponents) : base(actor, allies, opponents)
        {
        }
    }
}
