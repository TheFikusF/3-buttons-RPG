using RandN.Distributions;
using RPG.Data.Services;
using RPG.Entities;
using RPG.Fight.ActionResults;
using RPG.Items;
using RPG.Utils;

namespace RPG.Data
{
    public class Attack : IActionResult
    {
        public readonly Entity Attacker;
        public readonly Entity Target;

        public readonly int Amount;

        public readonly bool Killed;
        public readonly bool Missed;
        public readonly bool Evaded;
        public readonly bool Crit;

        public List<ItemUseResult> ItemUseResults { get; private init; }

        public bool Succes => !(Missed || Evaded) || Crit;

        public Attack(FightContext context, float multiplier = 1, bool attackersCallbacks = true, bool targetCallbacks = true)
        {
            Attacker = context.Actor;
            Target = context.Target;

            Amount = Target.Health.Value;

            Crit = Bernoulli.FromRatio((uint)Attacker.CritChance, 100).Sample(Extensions.RNG);
            Missed = !Bernoulli.FromRatio((uint)Attacker.HitChance, 100).Sample(Extensions.RNG);
            Evaded = Bernoulli.FromRatio((uint)Target.Evasion, 100).Sample(Extensions.RNG);

            int damage = Crit ? (int)(Attacker.Attack * Attacker.CritMultiplier) : Attacker.Attack;
            damage = !Crit && (Missed || Evaded) ? 0 : damage;

            if(Succes)
            {
                Target.TakeDamage((int)(damage * multiplier), Attacker);
            }

            ItemUseResults = new List<ItemUseResult>();
            Amount -= Target.Health.Value;

            context.Attack = this;

            if(attackersCallbacks)
            {
                ItemUseResults.AddRange(Attacker.Inventory.InvokeCallback(x => x.WearerAttacked, context));
            }

            if(targetCallbacks)
            {
                ItemUseResults.AddRange(Target.Inventory.InvokeCallback(x => x.WearerDamaged, FightContextService.Flip(context)));
            }
        }

        public override string ToString()
        {
            if (Killed)
            {
                return $"{Attacker.Name} killed {Target.Name}.";
            }

            if (Crit)
            {
                return $"{Attacker.Name} got critical hit on {Target.Name} for {Amount} HP!";
            }

            if (Evaded)
            {
                return $"{Target.Name} evaded {Attacker.Name}'s attack.";
            }

            if (Missed)
            {
                return $"{Attacker.Name} missed the attack on {Target.Name}.";
            }

            return $"{Attacker.Name} attacked {Target.Name} for {Amount} HP.";
        }
    }
}
