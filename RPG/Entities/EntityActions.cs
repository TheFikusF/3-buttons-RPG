namespace RPG.Entities
{
    internal class EntityActions
    {
        public class Spell
        {
            public readonly string Name;
            public readonly string Description;
            
            public readonly Action<Entity, List<Entity>> Action;
            public readonly int ManaCost;

            public Spell(string name, string description, Action<Entity, List<Entity>> action, int manaCost)
            {
                Name = name;
                Description = description;
                Action = action;
                ManaCost = manaCost;
            }

            public override string ToString() => $"{Name}{$"MP: {ManaCost}".PadLeft(23 - Name.Length)}{Environment.NewLine}|: {Description}";

            public string ShortToString() => $"{Name}";
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

        public void CastSpell(int slotIndex, Entity caster, List<Entity> oponents) => CastSpell(_spellSlots[slotIndex], caster, oponents);

        private void CastSpell(Spell spell, Entity caster, List<Entity> oponents)
        {
            if(caster.TryTakeMana(spell.ManaCost))
            {
                spell.Action(caster, oponents);
            }
        }
    }
}
