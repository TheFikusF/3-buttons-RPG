using RPG.GameStates;
using RPG.Items;
using RPG.Items.Serialization;
using RPG.Utils;

namespace RPG.Entities.NPCSpecializations
{
    public class Merchant : NPCSpecializtion
    {
        private const int _maxSellItem = 6;
        private const int _minSellItem = 3;

        public class SellableItem : IListStateItem
        {
            private Item _item;
            private int _price;

            public Item Item => _item;
            public int Price => _price;

            public SellableItem(Item item, int price)
            {
                _item = item;
                _price = price;
            }  
            
            public string GetFullString(int pad)
            {
                throw new NotImplementedException();
            }
        }

        private List<SellableItem> _itemsToSell;
        
        public override string Name => "Merchant";

        public override string Description => "Sells crazy things.";

        public List<SellableItem> SellableItems => _itemsToSell.ToList();

        public Merchant()
        {
            _itemsToSell = new List<SellableItem>();
        }

        public void RefreshSellList()
        {
            _itemsToSell = new List<SellableItem>();

            for(int i = 0; i < Extensions.GetRandom(_minSellItem, _maxSellItem); i++)
            {
                InventoryItem item = ItemsRepository.Items.ToList().PickRandom().Value.PickRandom();
                _itemsToSell.Add(new SellableItem(item, (int)((item.Defence + item.Attack + item.Health) * 1.5f)));
            }
        }
        
        public void SellItemToPlayer(Player player, int itemIndex)
        {
            if (itemIndex < 0 || itemIndex >= _itemsToSell.Count)
            {
                throw new IndexOutOfRangeException("The item index provided is out of range.");
            }

            SellableItem itemToSell = _itemsToSell[itemIndex];

            if (player.TryTakeMoney(_itemsToSell[itemIndex].Price))
            {
                //player.Inventory.AddToInventory()
            }
        }
    }
}
