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
        private static readonly MethodBase _attackConstructor = typeof(Attack).GetConstructor(new Type[] { typeof(Entity), typeof(Entity), typeof(float) });
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

        public static FightAction<ItemUseResult> GetLuaItemAction(string luaCode, string itemName)
        {
            return (user, opponents) =>
            {
                ItemUseResult result = new ItemUseResult($"{user.Name} used {itemName}");

                using (var lua = new Lua())
                {
                    lua.BasicInit(user, opponents, "user");

                    lua[nameof(result)] = result;

                    lua.DoString(luaCode);
                }

                return result;
            };
        }

        public static FightAction<ItemUseResult> GetLuaItemOnDamage(string luaCode, string itemName)
        {
            return (user, opponents) =>
            {
                ItemUseResult result = new ItemUseResult($"{user.Name} used {itemName}");

                using (var lua = new Lua())
                {
                    lua.BasicInit(user, opponents, "user");

                    lua[nameof(result)] = result;

                    lua.DoString(luaCode);
                }

                return result;
            };
        }

        public static FightAction<ItemUseResult, Attack> GetLuaItemOnAttack(string luaCode, string itemName)
        {
            return (user, opponents, attack) =>
            {
                ItemUseResult result = new ItemUseResult($"{user.Name} used {itemName}");

                using (var lua = new Lua())
                {
                    lua.BasicInit(user, opponents, "user");

                    lua[nameof(attack)] = attack;
                    lua[nameof(result)] = result;

                    lua.DoString(luaCode);
                }

                return result;
            };
        }

        public static FightAction<ItemUseResult, Spell, SpellResult> GetLuaItemOnSpellUse(string luaCode, string itemName)
        {
            return (user, opponents, spell, spellResult) =>
            {
                ItemUseResult result = new ItemUseResult($"{user.Name} used {itemName}");

                using (var lua = new Lua())
                {
                    lua.BasicInit(user, opponents, "user");

                    lua[nameof(result)] = result;
                    lua[nameof(spell)] = spell;
                    lua[nameof(spellResult)] = spellResult;

                    lua.DoString(luaCode);
                }

                return result;
            };
        }

        public static FightAction<ItemUseResult, ItemUseResult> GetLuaItemOnItemUse(string luaCode, string itemName)
        {
            return (user, opponents, itemUseResult) =>
            {
                ItemUseResult result = new ItemUseResult($"{user.Name} used {itemName}");

                using (var lua = new Lua())
                {
                    lua.BasicInit(user, opponents, "user");

                    lua[nameof(result)] = result;
                    lua[nameof(itemUseResult)] = itemUseResult;

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
