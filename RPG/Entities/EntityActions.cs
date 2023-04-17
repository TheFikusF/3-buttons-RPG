using RPG.Data;

namespace RPG.Entities
{
    public class EntityActions
    {
        public enum SpellResultType
        {
            Ok, NotEnoughMana,
        }

        public class Spell
        {
            public readonly string Name;
            public readonly string Description;
            
            public readonly Func<Entity, List<Entity>, SpellResult> Action;
            public readonly int ManaCost;

            public Spell(string name, string description, Func<Entity, List<Entity>, SpellResult> action, int manaCost)
            {
                Name = name;
                Description = description;
                Action = action;
                ManaCost = manaCost;
            }

            public static Spell Cleave() => new Spell("Cleave", "You do a cleave attack", (caster, opponents) =>
            {
                List<Attack> attacks = new List<Attack>();
                foreach(var oppnent in opponents)
                {
                    attacks.Add(new Attack(caster, oppnent, 0.4f));
                }

                return new SpellResult($"{caster.Name} cleaved {string.Join(", ",attacks.Where(x => !x.Missed && !x.Evaded).Select(x => $"{x.Target.Name} (-{x.Amount})" ))}.", SpellResultType.Ok);
            }, 10);

            public override string ToString() => $"{Name}{$"MP: {ManaCost}".PadLeft(23 - Name.Length)}{Environment.NewLine}|: {Description}";

            public string ShortToString() => $"{Name}";
        }

        public struct SpellResult
        {
            public readonly string Description;
            public readonly SpellResultType ResultType;

            public SpellResult(string description, SpellResultType resultType)
            {
                Description = description;
                ResultType = resultType;
            }

            public override string ToString() => Description;
        }

        private Spell[] _spellSlots;
        private Dictionary<string, (Spell, bool)> _spells;

        public int SlotsCount => _spellSlots.Length;

        public List<Spell> EquipedSpells => _spellSlots.ToList();
        public List<(Spell, bool)> Spells => _spells.Select(x => x.Value).ToList();

        public EntityActions(int maxSpells, Dictionary<string, (Spell, bool)> availableSpells = null) 
        {
            _spellSlots = new Spell[maxSpells];
            _spells = availableSpells ?? new Dictionary<string, (Spell, bool)>();
        }

        public void AddSpell(Spell spell) => _spells[spell.Name] = (spell, false);

        public void EquipSpell(string spellName, int slotIndex)
        {
            if (_spells.TryGetValue(spellName, out (Spell, bool) spell))
            {
                EquipSpell(spell.Item1, slotIndex);
            }
        }

        public void EquipSpell(Spell spell, int slotIndex)
        {
            if (!_spells.Any(x => x.Value.Item1 == spell))
            {
                AddSpell(spell);
            }

            if (_spellSlots[slotIndex] is not null)
            {
                _spells[_spellSlots[slotIndex].Name] = (_spellSlots[slotIndex], false);
            }

            _spells[spell.Name] = (spell, true);
            _spellSlots[slotIndex] = spell;
        }

        public SpellResult CastSpell(int slotIndex, Entity caster, List<Entity> oponents) => CastSpell(_spellSlots[slotIndex], caster, oponents);

        private SpellResult CastSpell(Spell spell, Entity caster, List<Entity> oponents)
        {
            if(caster.TryTakeMana(spell.ManaCost))
            {
                return spell.Action(caster, oponents);
            }

            return new SpellResult($"Not enough mana for {spell.Name}!", SpellResultType.NotEnoughMana);
        }
    }
}
