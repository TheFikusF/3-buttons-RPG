using RPG.Data;
using RPG.Entities.Serialization;
using RPG.Entities.Stats;
using RPG.Items;

namespace RPG.Entities
{
    public class EffectedEntity : Entity
    {
        private List<Effect> _effects;

        public override int Attack => GetMultipliedValue(base.Attack, EffectType.OutDamageAmp);

        public List<Effect> Effects => _effects.ToList();
        public bool Stunned => _effects.Any(x => x.Type == EffectType.Stun);

        public EffectedEntity(SerializedEntity serializedEntity, int level, int actionsSlots = 1) 
            : base(serializedEntity, level, actionsSlots)
        {
            _effects = new List<Effect>();
        }

        public EffectedEntity(string name, 
            int maxHealth, 
            Inventory inventory, 
            EntityStats stats, 
            EntityActions actions, 
            int level = 1, 
            int attack = 10, 
            float hitChance = 90, 
            float critMultiplier = 2.3F) 
            : base(name, maxHealth, inventory, stats, actions, level, attack, hitChance, critMultiplier)
        {
            _effects = new List<Effect>();
        }

        public override void Heal(int amount)
        {
            amount = GetMultipliedValue(amount, EffectType.InHealAmp);

            base.Heal(amount);
        }


        public override bool TryTakeDamage(int amount)
        {
            amount = GetMultipliedValue(amount, EffectType.InDamageAmp);

            var result = base.TryTakeDamage(amount);

            if(result)
            {
                Effects.Clear();
            }

            return result;
        }

        public List<string> TakeEffectsTurn()
        {
            var output = new List<string>();

            foreach (var effect in _effects)
            {
                output.Add(effect.TakeTurn(this));
            }

            _effects.RemoveAll(x => x.TurnsLeft == 0);
            return output.Where(x => !string.IsNullOrEmpty(x)).ToList();
        }

        public void ApplyEffect(Effect effect)
        {
            _effects.Add(effect);
        }

        private int GetMultipliedValue(int amount, EffectType type)
        {
            amount = (int)(amount * _effects.Aggregate(1f, (x, y) => y.Type == type ?
                y.Value * x : 1));

            return amount;
        }
    }
}
