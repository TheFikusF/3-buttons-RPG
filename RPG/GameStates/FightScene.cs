﻿using RPG.Data;
using RPG.Entities;
using RPG.Items;
using RPG.Items.Serialization;

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

        public override string GetStateText()
        {
            _turn++;
            string text = $"Turn #{_turn}" + Environment.NewLine;
            text += Player.ToShortString() + Environment.NewLine;
            text += new string('-', 35) + Environment.NewLine;
            text += string.Join(Environment.NewLine, _attacks) + Environment.NewLine;
            text += new string('-', 35) + Environment.NewLine;
            text += string.Join(Environment.NewLine, _enemies.Select(x => x.ToShortString()));
            
            return text;
        }

        public override GameState Button1()
        {
            _attacks = new List<Attack>();
            if (!_enemies.Any(x => x.Health.Value > 0))
            {
                ItemsRepository.TryGetItem("Zweihander", out Item item);
                return new RewardScreen(Player, 50, 50, new List<Item> { item });
            }

            _attacks.Add(new Attack(Player, _enemies.First(x => x.Health.Value > 0)));
            PerformEnemyTurn();

            return this;
        }

        public override GameState Button2()
        {
            _attacks = new List<Attack>();
            if (!_enemies.Any(x => x.Health.Value > 0))
            {
                ItemsRepository.TryGetItem("Zweihander", out Item item);
                return new RewardScreen(Player, 50, 50, new List<Item> { item });
            }

            _attacks.Add(new Attack(Player, _enemies.First(x => x.Health.Value > 0)));
            PerformEnemyTurn();

            return this;
        }

        private void PerformEnemyTurn()
        {
            foreach (Enemy enemy in _enemies)
            {
                if (enemy.Health.Value <= 0)
                {
                    continue;
                }
                _attacks.Add(new Attack(enemy, Player));
            }
        }
    }
}
