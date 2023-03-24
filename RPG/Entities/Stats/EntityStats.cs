namespace RPG.Entities.Stats
{
    public enum StatType
    {
        Agility, 
        Strength, 
        Inteligence
    }

    internal class EntityStats : GlobalStats<StatType> { }
}
