﻿using RPG.Data;
using RPG.Entities;
using RPG.Items;
using RPG.Items.Serialization;
using System.Text;

namespace RPG.GameStates
{
    internal class FightScene : GameState
    {
        private int _turn = 0;
        private List<Enemy> _enemies;
        private List<string> _turnLog;

        public FightScene(Player player, List<Enemy> enemies) : base(player)
        {
            _enemies = enemies;
            _turnLog = new List<string>();

            Button1Title = "Attack";
            Button2Title = $"{Player.Actions.EquipedSpells.First().Name}";
            Button3Title = $"{Player.Actions.EquipedSpells[1].Name}";
        }

        public override string GetStateText()
        {
            _turn++;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Turn #{_turn}");
            stringBuilder.AppendLine(Player.ToShortString());
            stringBuilder.AppendLine(new string('-', 35));
            stringBuilder.AppendLine(string.Join(Environment.NewLine, _turnLog));
            stringBuilder.AppendLine(new string('-', 35));
            stringBuilder.AppendLine(string.Join(Environment.NewLine, _enemies.Select(x => x.ToShortString())));

            return stringBuilder.ToString();
        }

        public override GameState Button1()
        {
            _turnLog = new List<string>();
            if (!_enemies.Any(x => x.Health.Value > 0))
            {
                ItemsRepository.TryGetItem("Zweihander", out Item item);
                return new RewardScreen(Player, 50, 50, new List<Item> { item });
            }

            _turnLog.Add(new Attack(Player, _enemies.First(x => x.Health.Value > 0)).ToString());
            PerformEnemyTurn();

            return this;
        }

        public override GameState Button2()
        {
            _turnLog = new List<string>();
            if (!_enemies.Any(x => x.Health.Value > 0))
            {
                ItemsRepository.TryGetItem("Zweihander", out Item item);
                return new RewardScreen(Player, 50, 50, new List<Item> { item });
            }

            EntityActions.SpellResult result = Player.CastSpell(0, _enemies.Where(x => x.Health.Value > 0).Cast<Entity>().ToList());
            _turnLog.Add(result.Description);

            if(result.ResultType == EntityActions.SpellResultType.NotEnoughMana)
            {
                return this;
            }

            PerformEnemyTurn();

            return this;
        }

        public override GameState Button3()
        {
            _turnLog = new List<string>();
            if (!_enemies.Any(x => x.Health.Value > 0))
            {
                ItemsRepository.TryGetItem("Zweihander", out Item item);
                return new RewardScreen(Player, 50, 50, new List<Item> { item });
            }

            EntityActions.SpellResult result = Player.CastSpell(1, _enemies.Where(x => x.Health.Value > 0).Cast<Entity>().ToList());
            _turnLog.Add(result.Description);

            if (result.ResultType == EntityActions.SpellResultType.NotEnoughMana)
            {
                return this;
            }

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
                _turnLog.Add(new Attack(enemy, Player).ToString());
            }
        }
    }
}
