using Newtonsoft.Json;
using RPG.Utils;

namespace RPG.Data
{
    public class FightAction<T> where T : class, new()
    {
        [JsonProperty("LuaCode")] private string _luaCode;

        [JsonIgnore] private Func<FightContext, T> _action;

        private bool _inited;

        [JsonConstructor]
        public FightAction()
        {

        }

        public FightAction(Func<FightContext, T> action) 
        {
            _action = action ?? (context => null);
        }

        public FightAction(string luaCode)
        {
            _luaCode = luaCode;

            Init();
        }

        public void Init()
        {
            _inited = true;

            if(string.IsNullOrEmpty(_luaCode))
            {
                _action = _action ?? (c => null);
                return;
            }

            _action = LuaExtensions.GetLuaAction<T>(_luaCode);
        }

        public T Invoke(FightContext context)
        {
            if(!_inited)
            {
                Init();
            }

            return _action.Invoke(context);
        }
    }
}
