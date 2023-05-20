using RPG.Entities;

namespace RPG.Data
{
    public enum EffectType
    {
        Burn, Poison, Stun, InDamageAmp, InHealAmp, OutDamageAmp, HealOT
    }

    public class Effect
    {
        public readonly EffectType Type;
        public readonly float Value;
        
        private int _turnsCount;

        public int TurnsLeft => _turnsCount;

        public Effect(EffectType type, float value, int turnsCount)
        {
            Type = type;
            Value = value;
            _turnsCount = turnsCount;
        }

        public string TakeTurn(Entity entity)
        {
            _turnsCount--;

            switch (Type)
            {
                case EffectType.Burn:
                    entity.TryTakeDamage((int)Value);
                    return $"{entity.Name} took {entity.LastDamageTook} burning!";
                case EffectType.Poison:
                    entity.TryTakeDamage((int)Value);
                    return $"{entity.Name} took {entity.LastDamageTook} decaying!";
                case EffectType.HealOT:
                    entity.Heal((int)Value);
                    return $"{entity.Name} healed for {entity.LastDamageTook} decaying!";
                default:
                    return string.Empty;
            }
        }

        public string ToString(Entity entity)
        {
            return Type switch
            {
                EffectType.Burn => $"{entity.Name} is burning. Takes {Value} damage for {TurnsLeft} turns.",
                EffectType.Poison => $"{entity.Name} is plagued. Takes {Value} damage for {TurnsLeft} turns.",
                EffectType.Stun => $"{entity.Name} is stuned and can't move for {TurnsLeft} turns.",
                EffectType.InDamageAmp => $"{entity.Name} is weakned. Takes x{Value} damage for {TurnsLeft} turns.",
                EffectType.InHealAmp => $"{entity.Name} takes increased heal. x{Value} heal for {TurnsLeft} turns.",
                EffectType.OutDamageAmp => $"{entity.Name} is courageous. Deals x{Value} damage for {TurnsLeft} turns.",
                EffectType.HealOT => $"{entity.Name} is regenerating. {Value} health for {TurnsLeft} turns.",
            };
        }
    }
}
