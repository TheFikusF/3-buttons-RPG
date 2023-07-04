using Newtonsoft.Json;
using RPG.Entities;
using RPG.Utils;

namespace RPG.Items
{
    [Serializable]
    public class UsableItem : Item
    {
        public class ItemUseResult
        {
            public readonly string Description;

            public ItemUseResult(string description)
            {
                Description = description;
            }
        }

        [JsonProperty("LuaCode")] private string _luaCode;

        [JsonIgnore] private Func<Entity, List<Entity>, ItemUseResult> _useCallback;

        public UsableItem(string name, Func<Entity, List<Entity>, ItemUseResult> useCallback) : base(name)
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
