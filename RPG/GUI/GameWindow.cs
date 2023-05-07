using RPG.Items.Serialization;
using RPG.Entities;
using RPG.Entities.Serialization;
using RPG.GameStates;
using Terminal.Gui;
using RPG.Data.DataProviders;

namespace RPG.GUI
{
    internal class GameWindow : Window
    {
        private Player _player;
        private GameState _currentState;

        private FrameView _frameView;

        private Button _button1;
        private Button _button2;
        private Button _button3;
        private TextView _textView;

        public GameWindow()
        {
            Title = "RPG (Ctrl+Q to quit)";
            InitializeComponents();

            ItemsRepository.InitFromJSON("Items.json");
            //new EntitiesRepository(JSONEntityProvider.InitFromJSON("Entities.json"));
            new EntitiesRepository(new EntityDataProvider());

            _player = new Player(SerializedEntity.FromJSON("Player.json"), 1);
            _player.Actions.EquipSpell(EntityActions.Spell.Cleave(), 0);

            SetCurrentState(new MainScreen(_player));
        }

        private void InitializeComponents()
        {
            _textView = new TextView()
            {
                ColorScheme = new ColorScheme()
                {
                    Normal = new Terminal.Gui.Attribute(Color.White, Color.Black)
                },
                Enabled = false,
                Text = "Password:",
                Height = Dim.Fill(),
                Width = Dim.Fill(),
                X = 0,
                Y = 0
            };

            _frameView = new FrameView()
            {
                Height = Dim.Fill() - 2,
                Width = Dim.Fill(),
                X = 0,
                Y = 0
            };

            _frameView.Add(_textView);

            _button1 = new Button()
            {
                Text = "Login",
                Y = Pos.Bottom(_frameView) + 1,
                X = Pos.Left(_frameView),
                IsDefault = true,
            };

            _button2 = new Button()
            {
                Text = "Login",
                Y = Pos.Bottom(_frameView) + 1,
                X = Pos.Center(),
                IsDefault = true,
            };

            _button3 = new Button()
            {
                Text = "Login",
                Y = Pos.Bottom(_frameView) + 1,
                X = Pos.Right(_frameView) - (11 + 11),
                IsDefault = true,
            };

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
            Add(_frameView, _button1, _button2, _button3);
        }

        private void RenderCurrentState()
        {
            _textView.Text = _currentState.GetStateText();
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
            _button3.X = Pos.Right(_frameView) - (width + 2);

            RenderCurrentState();
        }
    }
}
