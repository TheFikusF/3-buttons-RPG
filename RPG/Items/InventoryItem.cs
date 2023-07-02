using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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

        [JsonIgnore] public int Attack => _attack;
        [JsonIgnore] public int Defence => _defence;
        [JsonIgnore] public int Health => _health;
        [JsonIgnore] public List<SlotType> Slots => _slots.ToList();

        [JsonConstructor]
        private InventoryItem(string name, List<SlotType> slots, int attack = 0, int defence = 0, int health = 0) : base(name)
        {
            _attack = attack;
            _defence = defence;
            _health = health;
            _slots = slots;
        }

        public InventoryItem(InventoryItem item) : base(item.Name)
        {
            _attack = item.Attack;
            _defence = item.Defence;
            _health = item.Health;
            _slots = item.Slots;
        }

        public static InventoryItem TwoHanded(string name, int attack = 0, int defence = 0, int health = 0)
            => new InventoryItem(name, new List<SlotType>() { SlotType.Hand, SlotType.Hand }, attack, defence, health);

        public static InventoryItem OneHanded(string name, int attack = 0, int defence = 0, int health = 0)
            => new InventoryItem(name, new List<SlotType>() { SlotType.Hand }, attack, defence, health);

        public static InventoryItem Chest(string name, int attack = 0, int defence = 0, int health = 0)
            => new InventoryItem(name, new List<SlotType>() { SlotType.Chest }, attack, defence, health);

        public static InventoryItem Legs(string name, int attack = 0, int defence = 0, int health = 0)
            => new InventoryItem(name, new List<SlotType>() { SlotType.Legs }, attack, defence, health);

        public static InventoryItem Arms(string name, int attack = 0, int defence = 0, int health = 0)
            => new InventoryItem(name, new List<SlotType>() { SlotType.Arms }, attack, defence, health);

        public static InventoryItem Head(string name, int attack = 0, int defence = 0, int health = 0)
            => new InventoryItem(name, new List<SlotType>() { SlotType.Head }, attack, defence, health);
        
        public static InventoryItem Accesory(string name, int attack = 0, int defence = 0, int health = 0)
            => new InventoryItem(name, new List<SlotType>() { SlotType.Accessory }, attack, defence, health);

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
