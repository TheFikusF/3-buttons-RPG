﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace RPG.Entities.Stats
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum StatType
    {
        [EnumMember(Value = "Agility")] Agility,
        [EnumMember(Value = "Strength")] Strength,
        [EnumMember(Value = "Intelligence")] Intelligence
    }

    public class EntityStats : GlobalStats<StatType> 
    { 
        public EntityStats(int agility, int strength, int inteligence) : base()
        {
            SetLevel(StatType.Agility, agility);
            SetLevel(StatType.Strength, strength);
            SetLevel(StatType.Intelligence, inteligence);
        }

        public static EntityStats FromSerialized(Dictionary<StatType, int> stats)
        {
            stats.TryGetValue(StatType.Agility, out var agility);
            stats.TryGetValue(StatType.Strength, out var strength);
            stats.TryGetValue(StatType.Intelligence, out var inteligence);

            return new EntityStats(agility, strength, inteligence);
        }
    }
}
