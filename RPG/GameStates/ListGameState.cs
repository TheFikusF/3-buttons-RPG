using RPG.Entities;

namespace RPG.GameStates
{
    public class ListGameState : GameState
    {
        public class ItemsList<TItem> where TItem : class, IListStateItem
        {
            private bool _useLabels = false;
            private string _name = string.Empty;

            private List<TItem> _items;
            private Dictionary<string, TItem> _itemsDictionary;

            public int LabelWidth { get; set; } = 10;
            public string SelectedPrefix { get; set; } = "•>";
            public string DefaultPrefix { get; set; } = "|";

            private ItemsList(string name, bool useLabels)
            {
                _name = name;
                _useLabels = useLabels;
            }

            public ItemsList(List<TItem> items, string name = "") : this(name, false)
            {
                _items = items;
            }

            public ItemsList(Dictionary<string, TItem> itemsDictionary, string name = "") : this(name, true)
            {
                _itemsDictionary = itemsDictionary;
            }

            public virtual string ToString(int index)
            {
                return _useLabels ? GetLabeledListString(index) : GetListString(index);
            }

            protected virtual string GetListString(int index)
            {
                return string.Join(Environment.NewLine, _items.Select((x, i) => i == index ? 
                    $"{SelectedPrefix} {x.GetFullString(0)}" : $"{DefaultPrefix} {x}" ));
            }

            protected virtual string GetLabeledListString(int index)
            {
                return string.Join(Environment.NewLine, _items.Select((x, i) => i == index ?
                    $"{SelectedPrefix} {x.GetFullString(LabelWidth)}" : $"{DefaultPrefix} {x}"));
            }
        }

        public ListGameState(Player player) : base(player)
        {
        }

        public override string GetStateText()
        {
            throw new NotImplementedException();
        }
    }
}
