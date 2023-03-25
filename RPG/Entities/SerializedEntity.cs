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

        public static SerializedEntity FromJSON(string path) => JsonConvert.DeserializeObject<SerializedEntity>(File.ReadAllText(path));
    }

    [Serializable]
    public class EntitiesRepository
    {
        [JsonIgnore] private static EntitiesRepository _instance;
        
        [JsonProperty("Enemies")] private Dictionary<string, List<SerializedEntity>> _enemies;
        [JsonIgnore] private static Dictionary<string, List<SerializedEntity>> Enemies => _instance._enemies.ToDictionary(x => x.Key, x => x.Value);

        public static void InitFromJSON(string path)
        {
            _instance = JsonConvert.DeserializeObject<EntitiesRepository>(File.ReadAllText(path));
        }
    }
}
