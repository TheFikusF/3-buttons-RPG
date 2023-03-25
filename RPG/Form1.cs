using RPG.Entities;
using RPG.GameStates;
using RPG.Items;

namespace RPG
{
    public partial class Form1 : Form
    {
        private Player _player;
        private GameState _currentState;

        public Form1()
        {
            InitializeComponent();

            _player = new Player("Player", 70);
            _player.Inventory.Equip(Item.TwoHanded("Mega Sword", 10));
            _player.Inventory.AddToInventory(Item.OneHanded("Daggeer", 5));
            _player.Inventory.AddToInventory(Item.OneHanded("Daggeer1", 5));
            _player.Inventory.AddToInventory(Item.Head("Helmet", 0, 10));

            SetCurrentState(new MainScreen(_player));
        }

        private void ActionButton1_Click(object sender, EventArgs e)
        {
            SetCurrentState(_currentState.Button1());
        }

        private void ActionButton2_Click(object sender, EventArgs e)
        {
            SetCurrentState(_currentState.Button2());
        }

        private void ActionButton3_Click(object sender, EventArgs e)
        {
            SetCurrentState(_currentState.Button3());
        }

        private void RenderCurrentState()
        {
            _currentState.RenderState(MainTextBox);
        }

        private void SetCurrentState(GameState state)
        {
            if (_currentState != state)
            {
                _currentState = state;
            }

            ActionButton1.Text = _currentState.Button1Title;
            ActionButton2.Text = _currentState.Button2Title;
            ActionButton3.Text = _currentState.Button3Title;

            RenderCurrentState();
        }
    }
}