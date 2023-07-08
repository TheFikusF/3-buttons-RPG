using Newtonsoft.Json;
using RPG.Utils;
using Terminal.Gui;

namespace RPG.Items.Serialization
{
    [Serializable]
    internal class ItemsRepository
    {
        [JsonIgnore] private static ItemsRepository _instance;

        [JsonProperty("Enemies")] private Dictionary<string, List<InventoryItem>> _items;
        [JsonIgnore] private static Dictionary<string, List<InventoryItem>> Items => _instance._items.ToDictionary(x => x.Key, x => x.Value);

        public ItemsRepository()
        {

        }

        [JsonConstructor]
        public ItemsRepository(Dictionary<string, List<InventoryItem>> items)
        {
            _items = items;
        }

        public static void InitFromJSON(string path)
        {
            _instance = JsonConvert.DeserializeObject<ItemsRepository>(File.ReadAllText(path));
        }

        public static bool TryGetItem(string name, out InventoryItem item)
        {
            item = null;
            foreach(var key in _instance._items.SelectMany(x => x.Value))
            {
                if(key.Name == name)
                {
                    item = new InventoryItem(key);
                    return true;
                }
            }

            return false;
        }

        public static InventoryItem GetRandom() 
        {
            return new InventoryItem(Items.SelectMany(x => x.Value).ToList().PickRandom());
        }
    }
}
