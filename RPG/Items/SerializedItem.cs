﻿using Newtonsoft.Json;

namespace RPG.Items.Serialization
{
    [Serializable]
    internal class ItemsRepository
    {
        [JsonIgnore] private static ItemsRepository _instance;

        [JsonProperty("Enemies")] private Dictionary<string, List<Item>> _items;
        [JsonIgnore] public static Dictionary<string, List<Item>> Items => _instance._items.ToDictionary(x => x.Key, x => x.Value);

        public ItemsRepository()
        {

        }

        [JsonConstructor]
        public ItemsRepository(Dictionary<string, List<Item>> items)
        {
            _items = items;
        }

        public static void InitFromJSON(string path)
        {
            _instance = JsonConvert.DeserializeObject<ItemsRepository>(File.ReadAllText(path));
        }

        public static bool TryGetItem(string name, out Item item)
        {
            item = null;
            foreach(var key in _instance._items.SelectMany(x => x.Value))
            {
                if(key.Name == name)
                {
                    item = key;
                    return true;
                }
            }

            return false;
        }
    }
}
