namespace RPG.Entities.Stats
{
    public enum StatType
    {
        Agility, 
        Strength, 
        Inteligence
    }

    public class EntityStats : GlobalStats<StatType> 
    { 
        public EntityStats(int agility, int strength, int inteligence) : base()
        {
            SetLevel(StatType.Agility, agility);
            SetLevel(StatType.Strength, strength);
            SetLevel(StatType.Inteligence, inteligence);
        }
    }
}
