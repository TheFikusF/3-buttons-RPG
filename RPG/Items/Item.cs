using Newtonsoft.Json;
using RPG.GameStates;

namespace RPG.Items
{
    [Serializable]
    public abstract class Item : IListStateItem
    {
        [JsonProperty("Name")] private string _name;
        [JsonProperty("Description")] private string _description;

        [JsonIgnore] public string Name => _name;
        [JsonIgnore] public string Description => _description;


        public Item(string name)
        {
            _name = name;
        }

        public abstract string GetFullString();

        public abstract string GetFullString(int pad);
    }
}