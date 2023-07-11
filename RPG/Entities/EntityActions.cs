using RPG.Data;
using RPG.GameStates;
using Newtonsoft.Json;
using RPG.Fight.ActionResults;
using RPG.Items;

namespace RPG.Entities
{
    public class EntityActions
    {
        public enum SpellResultType
        {
            Ok, NotEnoughMana,
        }

        [Serializable]
        public class Spell : IListStateItem
        {
            [JsonProperty("Name")] public readonly string Name;
            [JsonProperty("Description")] public readonly string Description;
            [JsonProperty("ManaCost")] public readonly int ManaCost;

            [JsonProperty("Action")] private readonly FightAction<SpellResult> _action;

            public FightAction<SpellResult> Action => _action;

            public Spell(string name, string description, Func<FightContext, SpellResult> action, int manaCost)
            {
                Name = name;
                Description = description;
                _action = new FightAction<SpellResult>(action);
                ManaCost = manaCost;
            }

            public Spell(string name, string description, string luaAction, int manaCost)
            {
                Name = name;
                Description = description;
                _action = new FightAction<SpellResult>(luaAction);
                ManaCost = manaCost;

            }

            public static Spell Cleave() => new Spell("Cleave", "You do a cleave attack.", (context) =>
            {
                List<Attack> attacks = new List<Attack>();
                foreach(var opponent in context.Opponents)
                {
                    context.Target = opponent;
                    attacks.Add(new Attack(context, 0.4f, true, true));
                }

                return new SpellResult($"{context.Actor.Name} cleaved {string.Join(", ",attacks.Where(x => !x.Missed && !x.Evaded).Select(x => $"{x.Target.Name} (-{x.Amount})" ))}.", SpellResultType.Ok);
            }, 10);

            public static Spell ThunderStrike() => new Spell("ThunderStrike", "You strike enemies from the sky.", (context) =>
            {
                List<Attack> attacks = new List<Attack>();
                foreach (var opponent in context.Opponents)
                {
                    context.Target = opponent;
                    attacks.Add(new Attack(context, 1.4f, false, true));
                }

                return new SpellResult($"{context.Actor.Name} struck {string.Join(", ", attacks.Where(x => !x.Missed && !x.Evaded).Select(x => $"{x.Target.Name} (-{x.Amount})"))}.", SpellResultType.Ok);
            }, 25);

            public static Spell Heal() => new Spell("Heal", "Heal yourself for 3 rounds for 10 hp.", @"
                        result.Description = context.Actor.Name .. "" applied heal.""
                        result.ResultType = SpellResultType.Ok
                        
                        effect = Effect(6, 10, 3, caster)
                        context.Actor:ApplyEffect(effect)
                    ", 25);

            public override string ToString() => $"{Name}{$"MP: {ManaCost}".PadLeft(23 - Name.Length)}";

            public string ShortToString() => $"{Name}";

            public string GetFullString(int pad)
            {
                return ToString() + Environment.NewLine + $"{new string(' ', pad)}|| {Description}";
            }
        }

        public class SpellResult : IActionResult
        {
            public readonly string Description;
            public readonly SpellResultType ResultType;

            public List<ItemUseResult> ItemUseResults { get; set; }

            public SpellResult()
            {
                Description = "";
                ResultType = SpellResultType.Ok;
                ItemUseResults = new List<ItemUseResult>();
            }

            public SpellResult(string description, SpellResultType resultType)
            {
                Description = description;
                ResultType = resultType;
                ItemUseResults = new List<ItemUseResult>();
            }

            public override string ToString() => Description;
        }

        private Spell[] _spellSlots;
        private Dictionary<string, (Spell, bool)> _spells;

        public int SlotsCount => _spellSlots.Length;

        public List<Spell> EquippedSpells => _spellSlots.ToList();
        public List<(Spell, bool)> Spells => _spells.Select(x => x.Value).ToList();

        public EntityActions(int maxSpells, List<Spell> availableSpells = null) 
        {
            _spellSlots = new Spell[maxSpells];
            _spells = availableSpells is null ? new Dictionary<string, (Spell, bool)>() : availableSpells.ToDictionary(x => x.Name, x => (x, false));
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

        public SpellResult CastSpell(int slotIndex, FightContext context) => CastSpell(_spellSlots[slotIndex], context);

        private SpellResult CastSpell(Spell spell, FightContext context)
        {
            if(context.Actor.TryTakeMana(spell.ManaCost))
            {
                var result = spell.Action.Invoke(context);

                return result;
            }

            return new SpellResult($"Not enough mana for {spell.Name}!", SpellResultType.NotEnoughMana);
        }
    }
}
