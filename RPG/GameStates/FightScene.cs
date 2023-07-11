using Newtonsoft.Json.Linq;
using RPG.Data;
using RPG.Entities;
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
            Button2Title = $"{Player.Actions.EquippedSpells.First().Name}";
            Button3Title = $"{Player.Actions.EquippedSpells[1].Name}";
        }

        public override string GetStateText()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Turn #{_turn}");
            stringBuilder.AppendLine(Player.ToShortString());
            stringBuilder.AppendLine($":==={new string('-', 35)}");
            stringBuilder.AppendLine(string.Join(Environment.NewLine, _turnLog));
            stringBuilder.AppendLine($":==={new string('-', 35)}");
            stringBuilder.AppendLine(string.Join(Environment.NewLine + $"|{new string('-', 35)}" + Environment.NewLine, 
                _enemies.Select(x => $"{x.ToShortString()}" + (x.Effects.Count > 0 ?
                $"{ Environment.NewLine}{ string.Join(Environment.NewLine, x.Effects.Select(j => $"| {j.ToString(x)}"))}" : string.Empty))));

            return stringBuilder.ToString();
        }

        public override void ButtonPressStart()
        {
            _turn++;
            _turnLog = new List<string>();
        }

        public override void ButtonPressEnd()
        {
            CheckHealth();
        }

        public override GameState Button1()
        {
            if (!_enemies.Any(x => x.Health.Value > 0))
            {
                return GetRewardScreen();
            }

            if (CheckCanMove(Player))
            {
                Enemy enemy = _enemies.First(x => x.Health.Value > 0);
                Attack attack = new Attack(Player, enemy);
                _turnLog.Add(attack.ToString());

                if(attack.Succes)
                {
                    _turnLog.AddRange(Player.Inventory.InvokeCallback(x => x.WearerAttacked, new FightContext()
                    {
                        Actor = Player,
                        Target = enemy,
                        Opponents = _enemies.Except(new List<Enemy> { enemy }).Cast<Entity>().ToList(),
                        Attack = attack,
                    }).Select(x => x.Description));

                    _turnLog.AddRange(enemy.Inventory.InvokeCallback(x => x.WearerDamaged, new FightContext()
                    {
                        Actor = enemy,
                        Target = Player,
                        Allies = _enemies.Except(new List<Enemy> { enemy }).Cast<Entity>().ToList(),
                        Attack = attack,
                    }).Select(x => x.Description));
                }

            }
            
            AddToTurnLog(Player.TakeEffectsTurn());
            _turnLog.Add(string.Empty);

            PerformEnemiesTurn();

            return this;
        }

        public override GameState Button2()
        {
            if (!_enemies.Any(x => x.Health.Value > 0))
            {
                return GetRewardScreen();
            }

            CastSpell(0);
            return this;
        }

        public override GameState Button3()
        {
            if (!_enemies.Any(x => x.Health.Value > 0))
            {
                return GetRewardScreen();
            }

            CastSpell(1);
            return this;
        }

        private GameState GetRewardScreen()
        {
            int healthSum = _enemies.Sum(x => x.Health.MaxValue);
            return new RewardScreen(Player, (int)(healthSum * 0.5), (int)(healthSum * 0.5), _enemies.SelectMany(x => x.Inventory.Items).ToList());
        }

        private void CastSpell(int slot)
        {
            if(CheckCanMove(Player))
            {
                EntityActions.SpellResult result = Player.CastSpell(slot, _enemies.Where(x => x.Health.Value > 0).Cast<Entity>().ToList());
                _turnLog.Add(result.Description);

                if (result.ResultType == EntityActions.SpellResultType.NotEnoughMana)
                {
                    return;
                }
            }

            Player.TakeEffectsTurn();

            PerformEnemiesTurn();
        }

        private void PerformEnemiesTurn()
        {
            foreach (Enemy enemy in _enemies)
            {
                if (enemy.Health.Value <= 0)
                {
                    continue;
                }

                if(CheckCanMove(enemy))
                {
                    EnemyPerformAttack(enemy);
                }

                AddToTurnLog(enemy.TakeEffectsTurn());
                _turnLog.Add(string.Empty);
            }
        }

        private void EnemyPerformAttack(Enemy enemy)
        {
            var attack = new Attack(enemy, Player);
            _turnLog.Add(attack.ToString());

            if (attack.Succes)
            {
                _turnLog.AddRange(enemy.Inventory.InvokeCallback(x => x.WearerAttacked, new FightContext()
                {
                    Actor = enemy,
                    Target = Player,
                    Allies = _enemies.Except(new List<Enemy> { enemy }).Cast<Entity>().ToList(),
                    Attack = attack,
                }).Select(x => x.Description));

                _turnLog.AddRange(Player.Inventory.InvokeCallback(x => x.WearerDamaged, new FightContext()
                {
                    Actor = Player,
                    Target = enemy,
                    Opponents = _enemies.Except(new List<Enemy> { enemy }).Cast<Entity>().ToList(),
                    Attack = attack,
                }).Select(x => x.Description));
            }
        }

        private void CheckHealth()
        {
            if(_enemies.All(x => x.Health.Value <= 0))
            {
                Button1Title = "Continue";
                Button2Title = string.Empty;
                Button3Title = string.Empty;
            }
        }

        private bool CheckCanMove(EffectedEntity entity)
        {
            if(entity.Stunned)
            {
                _turnLog.Add($"{entity.Name} is stunned!");
            }

            return !entity.Stunned;
        }

        private void AddToTurnLog(List<string> logs)
        {
            if (logs.Count > 0)
            {
                _turnLog.Add(string.Join(Environment.NewLine, logs));
            }
        }
    }
}
