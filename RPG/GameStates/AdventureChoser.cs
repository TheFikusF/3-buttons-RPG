using RPG.Data;
using RPG.Entities;

namespace RPG.GameStates
{
    public class AdventureChoser : GameState
    {
        public AdventureChoser(Player player) : base(player)
        {
            Button1Title = "Adventure";
            Button2Title = "";
            Button3Title = "Dungeon";
        }

        public override void RenderState(TextBox textBox)
        {
           textBox.Text = $"Chouse where to go:";
        }

        public override GameState Button1()
        {
            //return new FightScene(Player, new List<Enemy>() { Enemy.Slime(1), Enemy.Slime(1) });
            return new DefaultAdventure(Player);
        }

        public override GameState Button3()
        {
            return base.Button3();
        }
    }
}
