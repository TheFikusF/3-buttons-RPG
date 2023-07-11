namespace RPG.Data.Services
{
    public static class FightContextService
    {
        public static FightContext Flip(FightContext context)
        {
            return new FightContext()
            {
                Actor = context.Target,
                Target = context.Actor,
                Opponents = context.Allies,
                Allies = context.Opponents,
                Attack = context.Attack,
                SpellUsed = context.SpellUsed,
                SpellUseResult = context.SpellUseResult,
                ItemUseResult = context.ItemUseResult,
            };
        }
    }
}
