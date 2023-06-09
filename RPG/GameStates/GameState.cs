﻿using RPG.Entities;

namespace RPG.GameStates
{
    public abstract class GameState
    {
        private Player _player;

        public Player Player => _player;
        public string Button1Title { get; protected set; }
        public string Button2Title { get; protected set; }
        public string Button3Title { get; protected set; }

        public GameState(Player player)
        {
            _player = player;
        }

        public abstract void RenderState(TextBox textBox);

        public virtual GameState Button1() { return this; }
        public virtual GameState Button2() { return this; }
        public virtual GameState Button3() { return this; }
    }
}
