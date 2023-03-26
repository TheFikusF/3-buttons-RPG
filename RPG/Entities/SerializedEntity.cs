using Newtonsoft.Json;
using RPG.Entities.Stats;
using RPG.Items;

namespace RPG.Entities.Serialization
{
    [Serializable]
    public struct SerializedEntity
    {
        public string Name;
        public int MaxHealth;
        public SlotType[] InventorySlots;
        public Dictionary<StatType, int> Stats;
        public int Attack;
        public float HitChance;
        public float CritMultiplier;
        public List<string> EquipedItems;
        public List<string> Inventory;

        public SerializedEntity()
        {

        }

        [JsonConstructor]
        public SerializedEntity(string name, 
            int maxHealth, 
            SlotType[] inventorySlots, 
            Dictionary<StatType, int> stats, 
            int attack, 
            float hitChance, 
            float critMultiplier,
            List<string> equipedItems,
            List<string> inventory)
        {
            Name = name;
            MaxHealth = maxHealth;
            InventorySlots = inventorySlots;
            Stats = stats;
            Attack = attack;
            HitChance = hitChance;
            CritMultiplier = critMultiplier;
            EquipedItems = equipedItems;
            Inventory = inventory;
        }

        public static SerializedEntity FromJSON(string path) => JsonConvert.DeserializeObject<SerializedEntity>(File.ReadAllText(path));
    }

    [Serializable]
    public class EntitiesRepository
    {
        [JsonIgnore] private static EntitiesRepository _instance;
        
        [JsonProperty("Enemies")] private Dictionary<string, List<SerializedEntity>> _enemies;
        [JsonIgnore] public static Dictionary<string, List<SerializedEntity>> Enemies => _instance._enemies.ToDictionary(x => x.Key, x => x.Value);

        public EntitiesRepository()
        {

        }

        [JsonConstructor]
        public EntitiesRepository(Dictionary<string, List<SerializedEntity>> enemies)
        {
            _enemies = enemies;
        }

        public static void InitFromJSON(string path)
        {
            _instance = JsonConvert.DeserializeObject<EntitiesRepository>(File.ReadAllText(path));
        }
    }
}
