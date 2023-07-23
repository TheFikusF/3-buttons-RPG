namespace RPG.Data.Services
{
    public static class FightContextService
    {
        public static FightContext Flip(FightContext context)
        {
            return context with 
            { 
                Actor = context.Actor,
                Target = context.Target,
                Opponents = context.Allies,
                Allies = context.Opponents
            };
        }
    }
}
