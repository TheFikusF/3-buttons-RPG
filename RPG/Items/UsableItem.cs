using Newtonsoft.Json;
using RPG.Entities;
using RPG.Utils;
using static RPG.Utils.Extensions;

namespace RPG.Items
{
    [Serializable]
    public class UsableItem : Item
    {
        [JsonProperty("LuaCode")] private string _luaCode;

        [JsonIgnore] private FightAction<ItemUseResult> _useCallback;

        public UsableItem(string name, FightAction<ItemUseResult> useCallback) : base(name)
        {
            _useCallback = useCallback;
        }

        public UsableItem(string name, string luaCode) : base(name)
        {
            _luaCode = luaCode;

            Init();
        }

        public void Init()
        {
            if(string.IsNullOrEmpty(_luaCode))
            {
                return;
            }

            _useCallback = LuaExtensions.GetLuaItemAction(_luaCode, Name);        
        }

        public ItemUseResult UseItem(Entity user, List<Entity> opponents)
        {
            return _useCallback?.Invoke(user, opponents) ?? throw new NullReferenceException("The action was empty");
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
