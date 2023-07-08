using RPG.Entities;
using RPG.Items;
using System.Text;

namespace RPG.GameStates
{
    internal class RewardScreen : GameState
    {
        private int _gold;
        private int _experience;

        private bool _goldGiven;
        private bool _levelUp;

        private Queue<Item> _items;

        private InventoryItem _currentItem;

        public RewardScreen(Player player, int experience, int gold, IEnumerable<Item> items) : base(player)
        {
            _experience = experience;
            _gold = gold;

            _levelUp = Player.AddExperience(experience);
            
            Button1Title = _levelUp ? "Level Up!" : "Continue";
            
            Player.AddMoney(gold);

            _items = new Queue<Item>(items);
        }

        public override string GetStateText()
        {
            var builder = new StringBuilder();
            builder.Append("You've got:");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);
            
            if(_goldGiven)
            {
                builder.Append(_currentItem.GetFullString());

                return builder.ToString();
            }

            builder.Append($"Gold: {_gold}");
            builder.Append(Environment.NewLine);
            builder.Append($"Experience: {_experience}" + (_levelUp ? $"{Environment.NewLine + Environment.NewLine} LEVEL UP!" : ""));

            return builder.ToString();
        }

        public override GameState Button1()
        {
            var item = _currentItem;

            var nextState = GetNextState();

            if (_levelUp && !_goldGiven)
            {
                _goldGiven = true;
                return new LevelUpScreen(Player, nextState);
            }

            if (_goldGiven && item != null)
            {
                Player.Inventory.AddToInventory(item);
                return nextState;
            }

            _goldGiven = true;
            return nextState;
        }

        public override GameState Button2()
        {
            if(!_goldGiven)
            {
                return this;
            }

            var item = _currentItem;

            var nextState = GetNextState();

            if (_goldGiven && item != null)
            {
                Player.Inventory.Equip(item);
                return nextState;
            }

            return nextState;
        }

        public override GameState Button3()
        {
            if (!_goldGiven)
            {
                return this;
            }

            var nextState = GetNextState();

            return nextState;
        }

        private GameState GetNextState()
        {
            bool got = _items.TryDequeue(out Item item) && (item is InventoryItem inventoryItem);
            if(got)
            {
                _currentItem = item as InventoryItem;
                DrawItemButtons();
            }
        
            return got ? this : new MainScreen(Player);
        }

        private void DrawItemButtons()
        {
            Button1Title = "Take";
            Button2Title = "Equip";
            Button3Title = "Skip";
        }
    }
}
