namespace RPG.Entities.Stats
{
    public enum StatType
    {
        Agility, 
        Strength, 
        Intelligence
    }

    public class EntityStats : GlobalStats<StatType> 
    { 
        public EntityStats(int agility, int strength, int inteligence) : base()
        {
            SetLevel(StatType.Agility, agility);
            SetLevel(StatType.Strength, strength);
            SetLevel(StatType.Intelligence, inteligence);
        }

        public static EntityStats FromSerialized(Dictionary<string, int> stats)
        {
            int agility = stats.DefaultIfEmpty(new KeyValuePair<string, int>("", 10)).FirstOrDefault(x => x.Key.Equals(StatType.Agility.ToString())).Value;
            int strength = stats.DefaultIfEmpty(new KeyValuePair<string, int>("", 10)).FirstOrDefault(x => x.Key.Equals(StatType.Strength.ToString())).Value;
            int inteligence = stats.DefaultIfEmpty(new KeyValuePair<string, int>("", 10)).FirstOrDefault(x => x.Key.Equals(StatType.Intelligence.ToString())).Value;
            
            return new EntityStats(agility, strength, inteligence);
        }
    }
}
