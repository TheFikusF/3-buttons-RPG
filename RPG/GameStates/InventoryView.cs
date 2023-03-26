using RPG.Entities;

namespace RPG.GameStates
{
    public class InventoryView : GameState
    {
        private enum InventoryViewState
        {
            List, Selection
        }

        private int _index = 0;
        private InventoryViewState _state = InventoryViewState.List;

        public InventoryView(Player player) : base(player)
        {
            UpdateButtonText();
        }

        public override string GetStateText()
        {
            return Player.Inventory.GetString(_index);
        }

        public override GameState Button1()
        {
            _state = _state == InventoryViewState.List ? InventoryViewState.Selection : InventoryViewState.List;
            UpdateButtonText();
            return this;
        }

        public override GameState Button2()
        {
            if(_state == InventoryViewState.Selection)
            {
                if(_index < Player.Inventory.SlotsCount)
                {
                    Player.Inventory.Unequip(_index);
                }
                else
                {
                    Player.Inventory.Equip(_index);
                    _index--;
                }

                _state = InventoryViewState.List;
                UpdateButtonText();
                return this;
            }


            if(_index == 0)
            {
                return new MainScreen(Player);
            }

            _index--;
            UpdateButtonText();
            return this;
        }

        public override GameState Button3()
        {
            if (_state == InventoryViewState.Selection)
            {
                return this;
            }

            if (_index < Player.Inventory.SlotsCount + Player.Inventory.InventorySize - 1)
            {
                _index++;
                UpdateButtonText();
            }
            return this;
        }

        private void UpdateButtonText()
        {
            switch(_state)
            {
                case InventoryViewState.List:
                    Button1Title = "Edit";
                    Button2Title = _index == 0 ? "Exit" : "Prev";
                    Button3Title = "Next";
                    break;
                case InventoryViewState.Selection:
                    Button1Title = "List";
                    Button2Title = _index < Player.Inventory.SlotsCount ? "Unequip" : "Equip";
                    Button3Title = "Delete";
                    break;
            }
        }
    }
}
