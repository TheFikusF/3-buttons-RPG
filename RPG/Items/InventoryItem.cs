using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RPG.Data;
using RPG.Entities;
using RPG.Utils;
using System.Runtime.Serialization;
using static RPG.Utils.Extensions;

namespace RPG.Items
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SlotType
    {
        [EnumMember(Value = "Hand")] Hand,
        [EnumMember(Value = "Feet")] Feet,
        [EnumMember(Value = "Legs")] Legs,
        [EnumMember(Value = "Chest")] Chest,
        [EnumMember(Value = "Arms")] Arms,
        [EnumMember(Value = "Head")] Head,
        [EnumMember(Value = "Accessory")] Accessory
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ItemRarity
    {
        [EnumMember(Value = "Common")] Common,
        [EnumMember(Value = "Uncommon")] Uncommon,
        [EnumMember(Value = "Rare")] Rare,
        [EnumMember(Value = "Epic")] Epic,
        [EnumMember(Value = "Legendary")] Legendary,
    }

    [Serializable]
    public class InventoryItem : Item
    {
        [JsonProperty("Attack")] private int _attack;
        [JsonProperty("Defence")] private int _defence;
        [JsonProperty("Health")] private int _health;
        [JsonProperty("InventorySlots")] private List<SlotType> _slots;

        [JsonProperty("WearerDamagedLuaCode")] private readonly string _wearerDamagedLuaCode;
        [JsonProperty("WearerAttackedLuaCode")] private readonly string _wearerAttackedLuaCode;
        [JsonProperty("WearerCastedSpellLuaCode")] private readonly string _wearerCastedSpellLuaCode;
        [JsonProperty("WearerItemUsedLuaCode")] private readonly string _wearerItemUsedLuaCode;

        [JsonIgnore] private FightAction<ItemUseResult> _onWearerDamaged;
        [JsonIgnore] private FightAction<ItemUseResult, Attack> _onWearerAttack;
        [JsonIgnore] private FightAction<ItemUseResult, EntityActions.Spell, EntityActions.SpellResult> _onWearerCastSpell;
        [JsonIgnore] private FightAction<ItemUseResult, ItemUseResult> _onWearerItemUsed;

        [JsonIgnore] public int Attack => _attack;
        [JsonIgnore] public int Defence => _defence;
        [JsonIgnore] public int Health => _health;
        [JsonIgnore] public List<SlotType> Slots => _slots.ToList();

        [JsonConstructor]
        private InventoryItem(string name, 
            List<SlotType> slots, 
            int attack = 0, 
            int defence = 0, 
            int health = 0) : base(name)
        {
            _attack = attack;
            _defence = defence;
            _health = health;
            _slots = slots;
        }

        private InventoryItem(string name,
            List<SlotType> slots,
            int attack = 0,
            int defence = 0,
            int health = 0,
            FightAction<ItemUseResult> onWearerDamaged = null,
            FightAction<ItemUseResult, Attack> onWearerAttack = null,
            FightAction<ItemUseResult, EntityActions.Spell, EntityActions.SpellResult> onWearerCastSpell = null,
            FightAction<ItemUseResult, ItemUseResult> onWearerItemUsed = null) 
            : this(name, slots, attack, defence, health)
        {
            _onWearerDamaged = onWearerDamaged;
            _onWearerAttack = onWearerAttack;
            _onWearerCastSpell = onWearerCastSpell;
            _onWearerItemUsed = onWearerItemUsed;
        }

        private InventoryItem(string name,
            List<SlotType> slots,
            int attack = 0,
            int defence = 0,
            int health = 0,
            string onWearerDamaged = null,
            string onWearerAttack = null,
            string onWearerCastSpell = null,
            string onWearerItemUsed = null)
            : this(name, slots, attack, defence, health)
        {
            _wearerDamagedLuaCode = onWearerDamaged;
            _wearerAttackedLuaCode = onWearerAttack;
            _wearerCastedSpellLuaCode = onWearerCastSpell;
            _wearerItemUsedLuaCode = onWearerItemUsed;

            Init();
        }

        public InventoryItem(InventoryItem item) : base(item.Name)
        {
            _attack = item.Attack;
            _defence = item.Defence;
            _health = item.Health;
            _slots = item.Slots;

            _onWearerDamaged = item._onWearerDamaged;
            _onWearerAttack = item._onWearerAttack;
            _onWearerCastSpell = item._onWearerCastSpell;
            _onWearerItemUsed = item._onWearerItemUsed;

            _wearerDamagedLuaCode = item._wearerDamagedLuaCode;
            _wearerAttackedLuaCode = item._wearerAttackedLuaCode;
            _wearerCastedSpellLuaCode = item._wearerCastedSpellLuaCode;
            _wearerItemUsedLuaCode = item._wearerItemUsedLuaCode;
        }

        public void Init()
        {
            if(!string.IsNullOrEmpty(_wearerDamagedLuaCode))
            {
                _onWearerDamaged = LuaExtensions.GetLuaItemOnDamage(_wearerDamagedLuaCode, Name);
            }

            if (!string.IsNullOrEmpty(_wearerAttackedLuaCode))
            {
                _onWearerAttack = LuaExtensions.GetLuaItemOnAttack(_wearerAttackedLuaCode, Name);
            }

            if (!string.IsNullOrEmpty(_wearerCastedSpellLuaCode))
            {
                _onWearerCastSpell = LuaExtensions.GetLuaItemOnSpellUse(_wearerCastedSpellLuaCode, Name);
            }

            if (!string.IsNullOrEmpty(_wearerItemUsedLuaCode))
            {
                _onWearerItemUsed = LuaExtensions.GetLuaItemOnItemUse(_wearerItemUsedLuaCode, Name);
            }
        }

        public override string ToString() => Name;

        public override string GetFullString() => Name + Environment.NewLine +
            $"Attack: {Attack}" + Environment.NewLine +
            $"Defence: {Defence}" + Environment.NewLine +
            $"Health: {Health}";

        public override string GetFullString(int pad) => Name + Environment.NewLine +
            $"Attack".PadLeft(pad) + $": {Attack}" + Environment.NewLine +
            $"Defence".PadLeft(pad) + $": {Defence}" + Environment.NewLine +
            $"Health".PadLeft(pad) + $": {Health}";
    }
}
