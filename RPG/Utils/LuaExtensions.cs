using RPG.Data;
using RPG.Entities;
using NLua;
using RPG.Items;
using System.Reflection;
using static RPG.Entities.EntityActions;
using static RPG.Utils.Extensions;

namespace RPG.Utils
{
    public static class LuaExtensions
    {
        private static readonly MethodBase _attackConstructor = typeof(Attack).GetConstructor(new Type[] { typeof(FightContext), typeof(float), typeof(bool), typeof(bool) });
        private static readonly MethodBase _effectConstructor = typeof(Effect).GetConstructor(new Type[] { typeof(int), typeof(float), typeof(int), typeof(Entity) });

        public static FightAction<SpellResult> GetLuaSpellAction(string luaCode, string spellName)
        {
            return (caster, opponents) =>
            {
                SpellResult result = new SpellResult($"{caster.Name} casted {spellName}", SpellResultType.Ok);

                using (var lua = new Lua())
                {
                    lua.BasicInit(caster, opponents, nameof(caster));

                    lua[nameof(SpellResultType)] = typeof(SpellResultType);
                    lua[nameof(result)] = result;

                    lua.DoString(luaCode);
                }

                return result;
            };
        }

        public static Func<FightContext, T> GetLuaAction<T>(string luaCode) where T : new()
        {
            return (context) =>
            {
                T result = new T();

                using (var lua = new Lua())
                {
                    lua[nameof(context)] = context;
                    lua[nameof(EffectType)] = typeof(EffectType);

                    lua.RegisterFunction(nameof(Attack), _attackConstructor);
                    lua.RegisterFunction(nameof(Effect), _effectConstructor);

                    lua[nameof(result)] = result;

                    lua.DoString(luaCode);
                }

                return result;
            };
        }

        public static void BasicInit(this Lua lua, Entity user, List<Entity> opponents, string userName = "user")
        {
            lua[userName] = user;
            lua[nameof(opponents)] = opponents;
            lua[nameof(EffectType)] = typeof(EffectType);

            lua.RegisterFunction(nameof(Attack), _attackConstructor);
            lua.RegisterFunction(nameof(Effect), _effectConstructor);
        }
    }
}
