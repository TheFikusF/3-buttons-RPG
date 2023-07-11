using RPG.Entities;

namespace RPG.Fight
{
    public class Fight
    {
        private Entity _actor;
        private List<Entity> _allies;
        private List<Entity> _opponents;

        private int _turn;
        private List<string> _log;

        public Fight(Entity actor, List<Entity> allies, List<Entity> opponents)
        {
            _actor = actor;
            _allies = allies;
            _opponents = opponents;

            _log = new List<string>();
        }
        
        public Fight(Entity actor) : this(actor, new List<Entity>(), new List<Entity>()) 
        {
        
        }
    }
}
