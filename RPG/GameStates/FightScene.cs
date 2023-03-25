using RPG.Data;
using RPG.Entities;
using Terminal.Gui;

namespace RPG.GameStates
{
    internal class FightScene : GameState
    {
        private int _turn = 0;
        private List<Enemy> _enemies;
        private List<Attack> _attacks;

        public FightScene(Player player, List<Enemy> enemies) : base(player)
        {
            _enemies = enemies;
            _attacks = new List<Attack>();
        }

        public override void RenderState(TextView textView)
        {
            _turn++;
            string text = $"Turn #{_turn}" + Environment.NewLine;
            text += Player.ToShortString() + Environment.NewLine;
            text += new string('-', 35) + Environment.NewLine;
            text += string.Join(Environment.NewLine, _attacks) + Environment.NewLine;
            text += new string('-', 35) + Environment.NewLine;
            text += string.Join(Environment.NewLine, _enemies.Select(x => x.ToShortString()));
            textView.Text = text;
        }

        public override GameState Button1()
        {
            _attacks = new List<Attack>();
            if(!_enemies.Any(x => x.Health > 0))
            {
                return new MainScreen(Player);
            }

            _attacks.Add(new Attack(Player, _enemies.First(x => x.Health > 0)));
            foreach(Enemy enemy in _enemies)
            {
                if(enemy.Health <= 0)
                {
                    continue;
                }
                _attacks.Add(new Attack(enemy, Player));
            }

            return this;
        }
    }
}
