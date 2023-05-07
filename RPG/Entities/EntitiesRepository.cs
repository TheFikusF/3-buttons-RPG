using RPG.Data.DataProviders;

namespace RPG.Entities.Serialization
{
    public class EntitiesRepository
    {
        private static EntitiesRepository _instance;
        
        private IEntityDataProvider _entityDataProvider;

        public static Dictionary<string, List<SerializedEntity>> Enemies => _instance._entityDataProvider.Enemies.ToDictionary(x => x.Key, x => x.Value);
        public static List<God> Gods => _instance._entityDataProvider.Gods.ToList();

        public EntitiesRepository(IEntityDataProvider entitiesProvider) 
        {
            _instance = this;
            _entityDataProvider = entitiesProvider;
        }
    }
}
