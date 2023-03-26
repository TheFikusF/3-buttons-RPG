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

    [Serializable]
    public class Item
    {
        [JsonProperty("Name")] private string _name;
        [JsonProperty("Attack")] private int _attack;
        [JsonProperty("Defence")] private int _defence;
        [JsonProperty("Health")] private int _health;
        [JsonProperty("InventorySlots")] private List<SlotType> _slots;

        [JsonIgnore] public string Name => _name;
        [JsonIgnore] public int Attack => _attack;
        [JsonIgnore] public int Defence => _defence;
        [JsonIgnore] public int Health => _health;
        [JsonIgnore] public List<SlotType> Slots => _slots.ToList();

        [JsonConstructor]
        private Item(string name, List<SlotType> slots, int attack = 0, int defence = 0, int health = 0) 
        {
            _name = name;
            _attack = attack;
            _defence = defence;
            _health = health;
            _slots = slots;
        }

        public Item(Item item)
        {
            _name = item.Name;
            _attack = item.Attack;
            _defence = item.Defence;
            _health = item.Health;
            _slots = item.Slots;
        }

        public static Item TwoHanded(string name, int attack = 0, int defence = 0, int health = 0)
            => new Item(name, new List<SlotType>() { SlotType.Hand, SlotType.Hand }, attack, defence, health);

        public static Item OneHanded(string name, int attack = 0, int defence = 0, int health = 0)
            => new Item(name, new List<SlotType>() { SlotType.Hand }, attack, defence, health);

        public static Item Chest(string name, int attack = 0, int defence = 0, int health = 0)
            => new Item(name, new List<SlotType>() { SlotType.Chest }, attack, defence, health);

        public static Item Legs(string name, int attack = 0, int defence = 0, int health = 0)
            => new Item(name, new List<SlotType>() { SlotType.Legs }, attack, defence, health);

        public static Item Arms(string name, int attack = 0, int defence = 0, int health = 0)
            => new Item(name, new List<SlotType>() { SlotType.Arms }, attack, defence, health);

        public static Item Head(string name, int attack = 0, int defence = 0, int health = 0)
            => new Item(name, new List<SlotType>() { SlotType.Head }, attack, defence, health);
        
        public static Item Accesory(string name, int attack = 0, int defence = 0, int health = 0)
            => new Item(name, new List<SlotType>() { SlotType.Accessory }, attack, defence, health);

        public override string ToString() => Name;

        public string GetFullString() => Name + Environment.NewLine +
            $"Attack: {Attack}" + Environment.NewLine +
            $"Defence: {Defence}" + Environment.NewLine +
            $"Health: {Health}";

        public string GetFullString(int pad) => Name + Environment.NewLine +
            $"Attack".PadLeft(pad) + $": {Attack}" + Environment.NewLine +
            $"Defence".PadLeft(pad) + $": {Defence}" + Environment.NewLine +
            $"Health".PadLeft(pad) + $": {Health}";
    }
}
