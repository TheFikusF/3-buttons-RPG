using RPG.Entities;
using Terminal.Gui;

namespace RPG.GameStates
{
    public class MainScreen : GameState
    {
        public MainScreen(Player player) : base(player)
        {
            Button1Title = "Go!";
            Button2Title = "Inventory";
            Button3Title = "";
        }

        public override string GetStateText()
        {
            return Player.ToString();
        }

        public override GameState Button1()
        {
            return new AdventureChoser(Player);
        }

        public override GameState Button2()
        {
            return new InventoryView(Player);
        }

        public override GameState Button3()
        {
            return new MarketScreen(Player, this);
        }
    }
}
