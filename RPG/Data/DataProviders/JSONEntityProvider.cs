using Newtonsoft.Json;
using RPG.Entities.Serialization;

namespace RPG.Data.DataProviders
{
    [Serializable]
    public class JSONEntityProvider : IEntityDataProvider
    {
        [JsonIgnore] public Dictionary<string, List<SerializedEntity>> Enemies => _enemies;

        [JsonIgnore] public List<God> Gods => throw new NotImplementedException();

        [JsonProperty("Enemies")] private Dictionary<string, List<SerializedEntity>> _enemies;

        [JsonConstructor]
        public JSONEntityProvider(Dictionary<string, List<SerializedEntity>> enemies)
        {
            _enemies = enemies;
        }

        public static JSONEntityProvider InitFromJSON(string path)
        {
            return JsonConvert.DeserializeObject<JSONEntityProvider>(File.ReadAllText(path));
        }
    }
}
