﻿namespace RPG.Items
{
    public class Inventory
    {
        private class InventorySlot
        {
            private SlotType _slotType;
            private Item _item;

            public SlotType Type => _slotType;
            public Item Item => _item;

            public InventorySlot(SlotType slotType, Item item = null)
            {
                _slotType = slotType;
                _item = item;
            }

            public Item Unequip()
            {
                Item item = _item;
                _item = null;
                return item;
            }

            public void Equip(Item item) => _item = item;
        }

        private List<InventorySlot> _slots;
        private List<Item> _inventory;

        public Item this[SlotType key]
        {
            get
            {
                var item = _slots.DefaultIfEmpty(null).FirstOrDefault(x => x.Type == key);
                if(item == null)
                {
                    return null;
                }

                return item.Item;
            }
        }
        public Item this[int index] => index >= 0 && index < _inventory.Count ? _inventory[index] : null;

        public int Attack => _slots.GroupBy(x => x.Item).Sum(x => x.Key == null ? 0 : x.Key.Attack);
        public int Defence => _slots.GroupBy(x => x.Item).Sum(x => x.Key == null ? 0 : x.Key.Defence);
        public int Health => _slots.GroupBy(x => x.Item).Sum(x => x.Key == null ? 0 : x.Key.Health);

        public int InventorySize => _inventory.Count;
        public int SlotsCount => _slots.Count;

        private Inventory(List<SlotType> slot)
        {
            _slots = slot.Select(x => new InventorySlot(x)).ToList();
            _inventory = new List<Item>();
        }

        public static Inventory Human() =>
            new Inventory(new List<SlotType>() { SlotType.Hand, SlotType.Hand,
                SlotType.Head,
                SlotType.Chest,
                SlotType.Arms,
                SlotType.Legs,
                SlotType.Feet,
                SlotType.Accessory, SlotType.Accessory, SlotType.Accessory });
    
        public void AddToInventory(Item item)
        {
            _inventory.Add(item);
        }

        public void Equip(int index)
        {
            var item = _inventory[index - SlotsCount];
            _inventory.RemoveAt(index - SlotsCount);
            Equip(item);
        }

        public void Equip(Item item)
        {
            foreach (var slotType in item.Slots)
            {
                if(_slots.Count(x => x.Type == slotType) < item.Slots.Count(x => x == slotType))
                {
                    return;
                }
            }

            var equiped = new List<InventorySlot>();
            foreach (var slotType in item.Slots)
            {
                var suitable = _slots.Except(equiped).Where(x => x.Type == slotType);
                if(suitable.Any(x => x.Item is null))
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

            void EquipItem(Item item, List<InventorySlot> equiped, IEnumerable<InventorySlot> suitable)
            {
                InventorySlot slot = suitable.First(x => x.Item == null);
                
                Item unequipedItem = slot.Unequip();
                if (unequipedItem is not null)
                {
                    _inventory.Add(unequipedItem);
                }
                
                slot.Equip(item);
                equiped.Add(slot);
            }
        }

        public void Unequip(int index) => Unequip(_slots[index].Unequip());

        public void Unequip(Item item)
        {
            if(item is null)
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

                if(index == selected)
                {
                    str += ">";
                }

                if(item.Item is null)
                {
                    str += " Empty";
                    return str;
                }

                if(index == selected)
                {
                    str += $" {item.Item.GetFullString(10)}";
                }

                if(index != selected) 
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
