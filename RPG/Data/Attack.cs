﻿using RPG.Entities;

namespace RPG.Data
{
    internal struct Attack
    {
        public readonly Entity Attacker;
        public readonly Entity Target;
        public readonly int Amount;
        public readonly bool Killed;
        public readonly bool Missed;
        public readonly bool Evaded;
        public readonly bool Crit;

        public Attack(Entity attacker, Entity target)
        {

            Attacker = attacker;
            Target = target;

            Amount = target.Health;
            Crit = attacker.CritChance > new Random().Next(100);
            Missed = attacker.HitChance <= new Random().Next(100);
            Evaded = target.HitChance > new Random().Next(100);

            int damage = Crit ? (int)(attacker.Attack * attacker.CritMultiplier) : attacker.Attack;
            damage = !Crit && (Missed || Evaded) ? 0 : damage;

            Killed = target.TryTakeDamage(damage);
            Amount -= target.Health;
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
                return $"{Target.Name} missed the attack on {Attacker.Name}.";
            }

            return $"{Attacker.Name} attacked {Target.Name} for {Amount} HP.";
        }
    }
}
