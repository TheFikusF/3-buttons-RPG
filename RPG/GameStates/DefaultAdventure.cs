using RPG.Data;
using RPG.Entities;
using Terminal.Gui;

namespace RPG.GameStates
{
    internal class DefaultAdventure : GameState
    {
        private int _adventureIndex = 0;
        private List<string> _adventureStrings = new List<string>();
        private List<AdventureEvent> _adventureEvents = new List<AdventureEvent>();
        private GameState _adventureFinalState;

        public DefaultAdventure(Player player) : base(player)
        {
            while (true)
            {
                var newEvent = new AdventureEvent();
                _adventureEvents.Add(newEvent);
                _adventureFinalState = newEvent.GetState(player, this);
                _adventureStrings.AddRange(newEvent.ActionDescriptions);
                
                if (_adventureFinalState != this)
                {
                    break;
                }
            }
        }

        public override string GetStateText()
        {
            return "• " + string.Join(Environment.NewLine + "• ", 
                _adventureEvents.Take(_adventureIndex + 1).SelectMany(x => x.ActionDescriptions)) + _adventureEvents[_adventureIndex].RandomNumber;
        }

        public override GameState Button1()
        {
            _adventureIndex++;
            return _adventureIndex == _adventureEvents.Count ? _adventureFinalState : this;
        }
    }
}
