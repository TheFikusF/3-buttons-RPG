using Newtonsoft.Json;
using RPG.Data;

namespace RPG.Items
{
    [Serializable]
    public class UsableItem : Item
    {
        [JsonProperty("UseCallback")] private FightContextAction<ItemUseResult> _useCallback;

        public UsableItem(string name, Func<FightContext, ItemUseResult> useCallback = null) : base(name)
        {
            _useCallback = new FightContextAction<ItemUseResult>(useCallback);
        }

        public UsableItem(string name, string luaCode) : base(name)
        {
            _useCallback = new FightContextAction<ItemUseResult>(luaCode);
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
