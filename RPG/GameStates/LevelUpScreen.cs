using RPG.Entities;
using RPG.Entities.Stats;

namespace RPG.GameStates
{
    internal class LevelUpScreen : GameState
    {
        private GameState _returnState;

        public LevelUpScreen(Player player, GameState returnState) : base(player)
        {
            _returnState = returnState;
            Button1Title = StatType.Agility.ToString();
            Button2Title = StatType.Strength.ToString();
            Button3Title = StatType.Intelligence.ToString();
        }

        public override string GetStateText()
        {
            return $"Level {Player.Level}! What would you like to upgrade?" + Environment.NewLine + Environment.NewLine + Player.Stats.ToString();
        }

        public override GameState Button1() => UpgradeStat(StatType.Agility);
        public override GameState Button2() => UpgradeStat(StatType.Strength);
        public override GameState Button3() => UpgradeStat(StatType.Intelligence);

        private GameState UpgradeStat(StatType stat)
        {
            Player.TryAddStatLevel(stat);
            
            if (Player.LevelUpPoints > 0) 
            {
                return this;
            }

            return _returnState;
        }
    }
}
