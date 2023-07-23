using RPG.Data;
using RPG.Entities;
using RPG.Fight.ActionResults;
using RPG.Items;

namespace RPG.Fight
{
    public class Fight
    {
        protected struct LogSkip : IActionResult
        {
            public List<ItemUseResult> ItemUseResults => new();

            public override string ToString()
            {
                return Environment.NewLine;
            }
        }

        private Entity _actor;
        private List<Entity> _allies;
        private List<Entity> _opponents;

        private List<IActionResult> _log;

        protected readonly LogSkip _logSkip = new LogSkip();

        public bool IsFightEnded => !_actor.Alive || !_opponents.Any(x => x.Alive);

        public Fight(Entity actor, List<Entity> allies, List<Entity> opponents)
        {
            _actor = actor;
            _allies = allies;
            _opponents = opponents;

            _log = new List<IActionResult>();
        }
        
        public Fight(Entity actor) : this(actor, new List<Entity>(), new List<Entity>()) 
        {
        
        }

        public void CastSpellActor(int spellIndex)
        {
            CastSpell(_actor, _allies, _opponents, spellIndex);
        }

        public void CastSpellAlly(int allyIndex, int spellIndex)
        {
            CastSpell(_allies[allyIndex], _allies.Where(x => x != _allies[allyIndex]).Append(_actor), _opponents, spellIndex);
        }

        public void CastSpellEnemy(int enemyIndex, int spellIndex)
        {
            CastSpell(_opponents[enemyIndex], _opponents.Where(x => x != _opponents[enemyIndex]), _allies.Append(_actor), spellIndex);
        }

        protected void CastSpell(Entity actor, IEnumerable<Entity> allies, IEnumerable<Entity> opponents, int spellIndex)
        {
            EntityActions.SpellResult spellResult = actor.CastSpell(spellIndex, new FightContext()
            {
                Actor = actor,
                Allies = allies.ToList(),
                Opponents = opponents.ToList(),
            });

            _log.Add(spellResult);
            _log.Add(_logSkip);
        }

        public void AttackActor(Entity target)
        {
            Attack(_actor, target,  _allies, _opponents);
        }

        public void AttackAlly(int allyIndex, Entity target)
        {
            Attack(_allies[allyIndex], 
                target, 
                _allies.Where(x => x != _allies[allyIndex]).Append(_actor), 
                _opponents.Where(x => x != target));
        }

        public void AttackEnemy(int enemyIndex, Entity target)
        {
            Attack(_opponents[enemyIndex],
                target,
                _opponents.Where(x => x != _opponents[enemyIndex]),
                _allies.Append(_actor).Where(x => x != target));
        }

        public void Attack(Entity actor, Entity target, IEnumerable<Entity> allies, IEnumerable<Entity> opponents)
        {
            var attack = new Attack(new FightContext()
            {
                Actor = actor,
                Target = target,
                Allies = allies.ToList(),
                Opponents = opponents.ToList(),
            });

            _log.Add(attack);
            _log.Add(_logSkip);
        }
    }
}
