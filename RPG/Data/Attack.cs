using RandN.Distributions;
using RPG.Entities;
using RPG.Utils;

namespace RPG.Data
{
    public class Attack
    {
        public readonly Entity Attacker;
        public readonly Entity Target;

        public readonly int Amount;

        public readonly bool Killed;
        public readonly bool Missed;
        public readonly bool Evaded;
        public readonly bool Crit;

        public Attack(Entity attacker, Entity target, float multiplier = 1)
        {
            Attacker = attacker;
            Target = target;

            Amount = target.Health.Value;

            Crit = Bernoulli.FromRatio((uint)attacker.CritChance, 100).Sample(Extensions.RNG);
            Missed = !Bernoulli.FromRatio((uint)attacker.HitChance, 100).Sample(Extensions.RNG);
            Evaded = Bernoulli.FromRatio((uint)target.Evasion, 100).Sample(Extensions.RNG);

            int damage = Crit ? (int)(attacker.Attack * attacker.CritMultiplier) : attacker.Attack;
            damage = !Crit && (Missed || Evaded) ? 0 : damage;

            if(!(Missed || Evaded) || Crit)
            {
                Killed = target.TryTakeDamage((int)(damage * multiplier), attacker);
            }

            Amount -= target.Health.Value;

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
