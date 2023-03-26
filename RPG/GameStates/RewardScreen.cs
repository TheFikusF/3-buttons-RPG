using RPG.Entities;
using Terminal.Gui;

namespace RPG.GameStates
{
    internal class RewardScreen : GameState
    {
        private int _experience;
        private int _gold;

        public RewardScreen(Player player, int experience, int gold) : base(player)
        {
            _experience = experience;
            _gold = gold;
        }

        public override string GetStateText()
        {
            return "";
        }
    }
}
