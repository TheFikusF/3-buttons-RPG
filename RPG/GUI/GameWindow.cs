using RPG.Entities;
using RPG.Entities.Serialization;
using RPG.GameStates;
using RPG.Items;
using Terminal.Gui;

namespace RPG.GUI
{
    internal class GameWindow : Window
    {
        private Player _player;
        private GameState _currentState;

        private Button _button1;
        private Button _button2;
        private Button _button3;
        private TextView _textView;

        public GameWindow()
        {
            Title = "Example App (Ctrl+Q to quit)";
            InitializeComponents();

            EntitiesRepository.InitFromJSON("Entities.json");
            _player = new Player(SerializedEntity.FromJSON("Player.json"), 1);

            _player.Inventory.Equip(Item.TwoHanded("Mega Sword", 10));
            _player.Inventory.AddToInventory(Item.OneHanded("Daggeer", 5));
            _player.Inventory.AddToInventory(Item.OneHanded("Daggeer1", 5));
            _player.Inventory.AddToInventory(Item.Head("Helmet", 0, 10));


            SetCurrentState(new MainScreen(_player));
        }

        private void InitializeComponents()
        {
            _textView = new TextView()
            {
                Text = "Password:",
                Height = Dim.Fill() - 2,
                Width = Dim.Fill(),
                X = 0,
                Y = 0
            };

            // Create login button
            _button1 = new Button()
            {
                Text = "Login",
                Y = Pos.Bottom(_textView) + 1,
                // center the login button horizontally
                X = Pos.Left(_textView),
                IsDefault = true,
            };

            _button2 = new Button()
            {
                Text = "Login",
                Y = Pos.Bottom(_textView) + 1,
                // center the login button horizontally
                X = Pos.Center(),
                IsDefault = true,
            };

            _button3 = new Button()
            {
                Text = "Login",
                Y = Pos.Bottom(_textView) + 1,
                // center the login button horizontally
                X = Pos.Right(_textView) - (Text.Length + 11),
                IsDefault = true,
            };

            // When login button is clicked display a message popup
            _button1.Clicked += () =>
            {
                SetCurrentState(_currentState.Button1());
            };

            _button2.Clicked += () =>
            {
                SetCurrentState(_currentState.Button2());
            };

            _button3.Clicked += () =>
            {
                SetCurrentState(_currentState.Button3());
            };

            // Add the views to the Window
            Add(_textView, _button1, _button2, _button3);
        }

        private void RenderCurrentState()
        {
            _currentState.RenderState(_textView);
        }

        private void SetCurrentState(GameState state)
        {
            if (_currentState != state)
            {
                _currentState = state;
            }

            _button1.Text = _currentState.Button1Title;
            _button2.Text = _currentState.Button2Title;
            _button3.Text = _currentState.Button3Title;
            _button3.GetCurrentWidth(out int width);
            _button3.X = Pos.Right(_textView) - (width + 2);

            RenderCurrentState();
        }
    }
}
