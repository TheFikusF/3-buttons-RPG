using RandN.Distributions;
using RPG.Entities;
using RPG.Entities.Serialization;
using RPG.GameStates;
using RPG.Utils;

namespace RPG.Data
{
    public enum EventType
    {
        Battle,
        Chest,
        Shrine,
        Trap,
    }

    internal class AdventureEvent
    {
        private struct EventData
        {
            public readonly string ButtonText;
            public readonly string EventText;
            public readonly int Chance;

            public EventData(string buttonText, string eventText, int chance)
            {
                ButtonText = buttonText;
                EventText = eventText;
                Chance = chance;
            }
        }

        private static readonly Dictionary<EventType, EventData> EventDataTable = new()
        {
            { EventType.Battle, new EventData("FIGHT!", "You've came across your hostile friends.", 50) },
            { EventType.Chest, new EventData("Open", "You've found a chest!", 5) },
            { EventType.Trap, new EventData("Continue", "You've traped into a pit with spikes.", 10) },
            { EventType.Shrine, new EventData("Continue", "You've found a magic shrine that cures your tormented body.", 50) },
        };

        private static Dictionary<EventType, float> _summedEventChances;
        private static Dictionary<EventType, float> SummedEventChances
        {
            get
            {
                if (_summedEventChances == null)
                {
                    _summedEventChances = new Dictionary<EventType, float>();

                    float sum = 0;
                    foreach (var e in EventDataTable)
                    {
                        sum += e.Value.Chance;
                        _summedEventChances.Add(e.Key, sum);
                    }
                }

                return _summedEventChances;
            }
        }

        public static EventType RandomEvent(out float randomNumber)
        {
            randomNumber = (float)(new Random().NextDouble() * SummedEventChances.Last().Value);

            foreach (var e in SummedEventChances)
            {
                if (randomNumber < e.Value)
                {
                    return e.Key;
                }
            }

            return EventType.Battle;
        }

        public readonly EventType Type;
        public readonly List<string> ActionDescriptions;
        public readonly float RandomNumber;
        public readonly string ButtonText;

        public AdventureEvent()
        {
            Type = RandomEvent(out float number);
            RandomNumber = number;
            ButtonText = EventDataTable[Type].ButtonText;

            ActionDescriptions = new List<string>();
            for(int i = 0; i < new Random().Next(3, 7); i++)
            {
                ActionDescriptions.Add("Wandering");
            }
        }

        public GameState GetState(Player player, GameState currentState)
        {
            ActionDescriptions.Add(EventDataTable[Type].EventText);
            GameState Heal()
            {
                player.Heal(10);
                return currentState;
            }

            GameState Trap()
            {
                player.TryTakeDamage(10, null);
                return currentState;
            }

            GameState Fight()
            {
                uint i = 100;
                var enemies = new List<Enemy>();
                while (Bernoulli.FromRatio(i, 100).Sample(Extensions.RNG))
                {
                    i = (uint)Math.Round(i * 0.4f);
                    enemies.Add(new Enemy(EntitiesRepository.Enemies["Default"].PickRandom(), player.Level + new Random().Next(-1, 4)));
                }

                return new FightScene(player, enemies);
            }

            GameState Chest()
            {
                return new MainScreen(player);
            }

            return Type switch
            {
                EventType.Battle => Fight(),
                EventType.Chest => Chest(),
                EventType.Shrine => Heal(),
                EventType.Trap => Trap(),
                _ => currentState,
            };
        }
    }
}
