using Newtonsoft.Json;

namespace RPG.Entities.Serialization
{
    [Serializable]
    public struct SerializedEntity
    {
        public string Name;
        public int MaxHealth;
        public Dictionary<string, string> InventorySlots;
        public Dictionary<string, int> Stats;
        public int Attack;
        public float HitChance;
        public float CritMultiplier;

        public SerializedEntity()
        {

        }

        [JsonConstructor]
        public SerializedEntity(string name, int maxHealth, Dictionary<string, string> inventorySlots, Dictionary<string, int> stats, int attack, float hitChance, float critMultiplier)
        {
            Name = name;
            MaxHealth = maxHealth;
            InventorySlots = inventorySlots;
            Stats = stats;
            Attack = attack;
            HitChance = hitChance;
            CritMultiplier = critMultiplier;
        }

        public static SerializedEntity FromJSON(string path) => JsonConvert.DeserializeObject<SerializedEntity>(File.ReadAllText(path));
    }

    [Serializable]
    public class EntitiesRepository
    {
        [JsonIgnore] private static EntitiesRepository _instance;
        
        [JsonProperty("Enemies")] private Dictionary<string, List<SerializedEntity>> _enemies;
        [JsonIgnore] private static Dictionary<string, List<SerializedEntity>> Enemies => _instance._enemies.ToDictionary(x => x.Key, x => x.Value);

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
