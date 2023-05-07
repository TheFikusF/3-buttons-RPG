using RPG.Entities;
using RPG.Entities.Serialization;

namespace RPG.Data.DataProviders
{
    public interface IEntityDataProvider
    {
        public Dictionary<string, List<SerializedEntity>> Enemies { get; }
        public List<God> Gods { get; }
    }
}
