using RPG.Entities;

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

        public override void RenderState(TextBox textBox)
        {
            textBox.Text = Player.ToString();
        }

        public override GameState Button1()
        {
            return new AdventureChoser(Player);
        }

        public override GameState Button2()
        {
            return new InventoryView(Player);
        }
    }
}
