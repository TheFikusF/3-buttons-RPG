namespace RPG.Items
{
    public enum SlotType
    {
        Hand, Feet, Legs, Chest, Arms, Head, Accessory
    }

    public class Item
    {
        private string _name;
        private int _attack;
        private int _defence;
        private int _health;
        private List<SlotType> _slots;

        public string Name => _name;
        public int Attack => _attack;
        public int Defence => _defence;
        public int Health => _health;
        public List<SlotType> Slots => _slots.ToList();

        private Item(string name, List<SlotType> slots, int attack = 0, int defence = 0, int health = 0) 
        {
            _name = name;
            _attack = attack;
            _defence = defence;
            _health = health;
            _slots = slots;
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
