using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RPG.Data;
using System.Runtime.Serialization;

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

        [JsonProperty("WearerDamaged")]      public readonly FightAction<ItemUseResult> WearerDamaged;
        [JsonProperty("WearerAttacked")]     public readonly FightAction<ItemUseResult> WearerAttacked;
        [JsonProperty("WearerCastedSpell")]  public readonly FightAction<ItemUseResult> WearerCastedSpell;
        [JsonProperty("WearerItemUsed")]     public readonly FightAction<ItemUseResult> WearerItemUsed;

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
            Func<FightContext, ItemUseResult> onWearerDamaged = null, 
            Func<FightContext, ItemUseResult> onWearerAttacked = null, 
            Func<FightContext, ItemUseResult> onWearerCastSpell = null,
            Func<FightContext, ItemUseResult> onWearerItemUsed = null) 
            : this(name, slots, attack, defence, health)
        {
            WearerDamaged = new FightAction<ItemUseResult>(onWearerDamaged);
            WearerAttacked = new FightAction<ItemUseResult>(onWearerAttacked);
            WearerCastedSpell = new FightAction<ItemUseResult>(onWearerCastSpell);
            WearerItemUsed = new FightAction<ItemUseResult>(onWearerItemUsed);
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
            WearerDamaged = new FightAction<ItemUseResult>(onWearerDamaged);
            WearerAttacked = new FightAction<ItemUseResult>(onWearerAttack);
            WearerCastedSpell = new FightAction<ItemUseResult>(onWearerCastSpell);
            WearerItemUsed = new FightAction<ItemUseResult>(onWearerItemUsed);
        }

        public InventoryItem(InventoryItem item) : base(item.Name)
        {
            _attack = item.Attack;
            _defence = item.Defence;
            _health = item.Health;
            _slots = item.Slots;

            WearerDamaged = item.WearerDamaged;
            WearerAttacked = item.WearerAttacked;
            WearerCastedSpell = item.WearerCastedSpell;
            WearerItemUsed = item.WearerItemUsed;
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
