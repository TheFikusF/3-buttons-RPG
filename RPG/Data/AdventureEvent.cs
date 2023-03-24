using RPG.Entities;
using RPG.GameStates;

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
        private static readonly Dictionary<EventType, float> EventChances = new()
        {
            { EventType.Battle, 50 },
            { EventType.Chest, 5 },
            { EventType.Trap, 10 },
            { EventType.Shrine, 10 },
        };

        private static Dictionary<EventType, float> _summedEventChances;
        private static Dictionary<EventType, float> SummedEventChances
        {
            get
            {
                if (_summedEventChances == null)
                {
                    _summedEventChances = new();

                    float sum = 0;
                    foreach (var e in EventChances)
                    {
                        sum += e.Value;
                        _summedEventChances.Add(e.Key, sum);
                    }
                }

                return _summedEventChances;
            }
        }

        public static EventType RandomEvent(out float randomNumber)
        {
            randomNumber = (float)(new Random().NextDouble() * SummedEventChances.Last().Value);
            var previousType = SummedEventChances.First().Key;

            foreach (var e in SummedEventChances)
            {
                if (randomNumber < e.Value)
                {
                    return e.Key;
                }
                previousType = e.Key;
            }

            return EventType.Battle;
        }

        public readonly EventType Type;
        public readonly List<string> ActionDescriptions;
        public readonly float RandomNumber;

        public AdventureEvent()
        {
            Type = RandomEvent(out float number);
            RandomNumber = number;
            ActionDescriptions = new List<string>();
            for(int i = 0; i < new Random().Next(3, 7); i++)
            {
                ActionDescriptions.Add("Wandering");
            }
        }

        public GameState GetState(Player player, DefaultAdventure currentAdventure)
        {
            GameState Heal()
            {
                player.Heal(10);
                ActionDescriptions.Add("Heal");
                return currentAdventure;
            }

            GameState Trap()
            {
                player.TryTakeDamage(10);
                ActionDescriptions.Add("Trap");
                return currentAdventure;
            }

            GameState Fight()
            {
                ActionDescriptions.Add("Fight!");
                return new FightScene(player, new List<Enemy>() { Enemy.Slime(1), Enemy.Slime(1) });
            }

            GameState Chest()
            {
                ActionDescriptions.Add("Chest!");
                return new MainScreen(player);
            }

            return Type switch
            {
                EventType.Battle => Fight(),
                EventType.Chest => Chest(),
                EventType.Shrine => Heal(),
                EventType.Trap => Trap(),
                _ => currentAdventure,
            };
        }
    }
}
