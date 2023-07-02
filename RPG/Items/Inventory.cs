using System.Text.RegularExpressions;

namespace RPG.Items
{
    public class Inventory
    {
        private class InventorySlot
        {
            private SlotType _slotType;
            private InventoryItem? _item;

            public SlotType Type => _slotType;
            public InventoryItem? Item => _item;

            public InventorySlot(SlotType slotType, InventoryItem? item = null)
            {
                _slotType = slotType;
                _item = item;
            }

            public InventoryItem? Unequip()
            {
                InventoryItem? item = _item;
                _item = null;
                return item;
            }

            public void Equip(InventoryItem item) => _item = item;
        }

        private List<InventorySlot> _slots;
        private List<Item> _inventory;

        public InventoryItem? this[SlotType key]
        {
            get
            {
                var item = _slots.DefaultIfEmpty(null).FirstOrDefault(x => x.Type == key);

                return item?.Item;
            }
        }
        public Item? this[int index] => index >= 0 && index < _inventory.Count ? _inventory[index] : null;

        public int Attack => _slots.GroupBy(x => x.Item).Sum(x => x.Key == null ? 0 : x.Key.Attack);
        public int Defence => _slots.GroupBy(x => x.Item).Sum(x => x.Key == null ? 0 : x.Key.Defence);
        public int Health => _slots.GroupBy(x => x.Item).Sum(x => x.Key == null ? 0 : x.Key.Health);

        public int InventorySize => _inventory.Count;
        public int SlotsCount => _slots.Count;
        public List<Item> Items => _inventory.ToList();

        public Inventory(IEnumerable<SlotType> slot)
        {
            _slots = slot.Select(x => new InventorySlot(x)).ToList();
            _inventory = new List<Item>();
        }

        public static Inventory FromSerialized(Dictionary<string, string> slots)
        {
            var newSlots = slots.Where(x => Enum.TryParse(Regex.Replace(x.Key, @"[\d-]", string.Empty), out SlotType type)).Select(x => Enum.Parse<SlotType>(Regex.Replace(x.Key, @"[\d-]", string.Empty)));
            return new Inventory(newSlots);
        }

        public static Inventory Human() => new Inventory(HumanSlots());

        public static List<SlotType> HumanSlots() =>
            new List<SlotType>() { SlotType.Hand, SlotType.Hand,
                SlotType.Head,
                SlotType.Chest,
                SlotType.Arms,
                SlotType.Legs,
                SlotType.Feet,
                SlotType.Accessory, SlotType.Accessory, SlotType.Accessory };

        public void AddToInventory(InventoryItem item)
        {
            _inventory.Add(item);
        }

        public void Equip(int index)
        {

            var item = _inventory[index - SlotsCount];
            if(item is InventoryItem inventoryItem)
            {
                _inventory.RemoveAt(index - SlotsCount);
                Equip(inventoryItem);
            }
        }

        public void Equip(InventoryItem item)
        {
            foreach (var slotType in item.Slots)
            {
                if (_slots.Count(x => x.Type == slotType) < item.Slots.Count(x => x == slotType))
                {
                    return;
                }
            }

            var equiped = new List<InventorySlot>();
            foreach (var slotType in item.Slots)
            {
                var suitable = _slots.Except(equiped).Where(x => x.Type == slotType);
                if (suitable.Any(x => x.Item is null))
                {
                    EquipItem(item, equiped, suitable);
                    continue;
                }

                if (suitable.Any())
                {
                    InventorySlot slot = suitable.First();
                    Unequip(slot.Unequip());
                    slot.Equip(item);
                    equiped.Add(slot);
                    continue;
                }
            }

            void EquipItem(InventoryItem item, List<InventorySlot> equiped, IEnumerable<InventorySlot> suitable)
            {
                InventorySlot slot = suitable.First(x => x.Item == null);

                InventoryItem? unequipedItem = slot.Unequip();
                if (unequipedItem is not null)
                {
                    _inventory.Add(unequipedItem);
                }

                slot.Equip(item);
                equiped.Add(slot);
            }
        }

        public void Unequip(int index) => Unequip(_slots[index].Unequip());

        public void Unequip(InventoryItem? item)
        {
            if (item is null)
            {
                return;
            }

            foreach (var slot in _slots.Where(value => value.Item == item))
            {
                slot.Unequip();
            }

            _inventory.Add(item);
        }

        public string GetString(int selected)
        {
            string GetItemString(InventorySlot item, int index)
            {
                string str = $"{item.Type.ToString().PadLeft(12)}|";

                if (index == selected)
                {
                    str += ">";
                }

                if (item.Item is null)
                {
                    str += " Empty";
                    return str;
                }

                if (index == selected)
                {
                    str += $" {item.Item.GetFullString(10)}";
                }

                if (index != selected)
                {
                    str += $" {item.Item}";
                }

                return str;
            }

            return $"Equiped: " + Environment.NewLine +
                string.Join("", _slots.Select((value, index) => GetItemString(value, index) + Environment.NewLine)) +
                $"Inventory: " + Environment.NewLine +
                string.Join("", _inventory.Select((value, index) => index == selected - _slots.Count ?
                $"|> {value.GetFullString()}" : $"| {value}" + Environment.NewLine));
        }
    }
}
