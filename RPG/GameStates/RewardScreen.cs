using RPG.Entities;
using RPG.Items;
using System.Text;

namespace RPG.GameStates
{
    internal class RewardScreen : GameState
    {
        private bool _goldGiven;

        private int _experience;
        private int _gold;
        private Queue<Item> _items;

        private Item _currentItem;

        public RewardScreen(Player player, int experience, int gold, List<Item> items) : base(player)
        {
            _experience = experience;
            _gold = gold;
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
                _currentItem = _items.Dequeue();

                builder.Append(_currentItem.GetFullString());

                Button1Title = "Take";
                Button2Title = "Equip";
                Button1Title = "Skip";

                return builder.ToString();
            }

            builder.Append($"Gold: {_gold}");
            builder.Append($"Experience: {_experience}");
            
            return builder.ToString();
        }

        public override GameState Button1()
        {
            return this;
        }

        public override GameState Button2()
        {
            return this;
        }

        public override GameState Button3()
        {
            return this;
        }
    }
}
