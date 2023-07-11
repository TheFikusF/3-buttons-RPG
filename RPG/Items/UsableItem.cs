using Newtonsoft.Json;
using RPG.Data;

namespace RPG.Items
{
    [Serializable]
    public class UsableItem : Item
    {
        [JsonProperty("UseCallback")] private FightAction<ItemUseResult> _useCallback;

        public UsableItem(string name, Func<FightContext, ItemUseResult> useCallback = null) : base(name)
        {
            _useCallback = new FightAction<ItemUseResult>(useCallback);
        }

        public UsableItem(string name, string luaCode) : base(name)
        {
            _useCallback = new FightAction<ItemUseResult>(luaCode);
        }

        public override string GetFullString()
        {
            throw new NotImplementedException();
        }

        public override string GetFullString(int pad)
        {
            throw new NotImplementedException();
        }
    }
}
