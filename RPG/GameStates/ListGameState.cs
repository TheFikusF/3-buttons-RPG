using RPG.Entities;
using System.Text;

namespace RPG.GameStates
{
    public abstract class ListGameState : GameState
    {
        private enum ListViewState
        {
            List, Selection
        }

        public class LabeledListStateItem : IListStateItem
        {
            public string Name { get; set; }
            public IListStateItem Item { get; set; }

            public LabeledListStateItem(string name, IListStateItem item)
            {
                Name = name;
                Item = item;
            }

            public string GetFullString(int pad)
            {
                return Item.GetFullString(pad);
            }
        }

        public class ItemsList
        {
            private readonly bool _useLabels = false;
            private string _name = string.Empty;

            private List<IListStateItem> _items;

            private List<Action<IListStateItem, int>> _interactionCallbacks;

            public List<IListStateItem> Items => _items;

            public int LabelWidth { get; set; } = 10;
            public string SelectedPrefix { get; set; } = "•>";
            public string DefaultPrefix { get; set; } = "|";

            public int ItemsCount => _items.Count;

            private ItemsList(string name, bool useLabels)
            {
                _name = name;
                _useLabels = useLabels;
            }

            public ItemsList(IEnumerable<IListStateItem> items, string name = "") : this(name, items.GetType().GetGenericArguments()[0] == typeof(LabeledListStateItem))
            {
                _items = items.ToList();
            }

            public void SetInteractionCallbacks(List<Action<IListStateItem, int>> interactionCallbacks) => _interactionCallbacks = interactionCallbacks;

            public void Interact(int interactionIndex, int itemIndex)
            {
                if (interactionIndex >= _interactionCallbacks.Count)
                {
                    return;
                }

                _interactionCallbacks[interactionIndex]?.Invoke(_useLabels ? (_items[itemIndex] as LabeledListStateItem).Item : _items[itemIndex], itemIndex);
            }

            public virtual string ToString(int index)
            {
                var str = _name + Environment.NewLine + new string('-', 35) + Environment.NewLine;
                return str + (_useLabels ? GetLabeledListString(index) : GetListString(index));
            }

            protected virtual string GetListString(int index)
            {
                return string.Join(Environment.NewLine, _items.Select((x, i) => i == index ?
                    $"{SelectedPrefix} {x.GetFullString(0)}" : $"{DefaultPrefix} {x}"));
            }

            protected virtual string GetLabeledListString(int index)
            {
                string GetItemString(LabeledListStateItem item, int currentIndex)
                {
                    string str = $"{item.Name.PadLeft(LabelWidth + 2)}|";

                    if (index == currentIndex)
                    {
                        str += SelectedPrefix;
                    }

                    if (item is null)
                    {
                        str += " Empty";
                        return str;
                    }

                    if (index == currentIndex)
                    {
                        str += $" {item.Item.GetFullString(LabelWidth)}";
                    }

                    if (index != currentIndex)
                    {
                        str += $" {item.Item}";
                    }

                    return str;
                }

                return string.Join(Environment.NewLine, _items.Select((x, i) => $"{GetItemString(x as LabeledListStateItem, i)}"));
            }
        }

        private List<ItemsList> _lists;

        private ListViewState _state;
        private int _index;

        private GameState _stateToExit;

        public ListGameState(Player player, GameState stateToExit) : base(player)
        {
            _lists = new();
            _stateToExit = stateToExit;
        }

        protected void SetupList(ItemsList list, List<Action<IListStateItem, int>> interactionCallbacks)
        {
            _lists.Add(list);
            list.SetInteractionCallbacks(interactionCallbacks);
        }

        public override string GetStateText()
        {
            var builder = new StringBuilder();

            int prevListsItemsCount = 0;
            foreach (var list in _lists)
            {
                builder.Append(list.ToString(_index - prevListsItemsCount));
                builder.Append(Environment.NewLine + Environment.NewLine);
                prevListsItemsCount += list.ItemsCount;
            }

            return builder.ToString();
        }

        public override GameState Button1()
        {
            _state = _state switch
            {
                ListViewState.List => ListViewState.Selection,
                ListViewState.Selection => ListViewState.List,
                _ => ListViewState.List,
            };
            return this;
        }

        public override GameState Button2()
        {
            switch (_state)
            {
                case ListViewState.List when _index == 0:
                    return _stateToExit;
                case ListViewState.List when _index != 0:
                    _index--;
                    return this;
                case ListViewState.Selection:
                    InteractWithCurrentItem(0);
                    return this;
                default:
                    return this;
            }
        }

        public override GameState Button3()
        {
            switch (_state)
            {
                case ListViewState.List:
                    _index++;
                    return this;
                case ListViewState.Selection:
                    InteractWithCurrentItem(1);
                    return this;
                default:
                    return this;
            }
        }

        private void InteractWithCurrentItem(int interactionIndex)
        {
            (int listIndex, int itemIndex) = CurrentListIndex();
            _lists[listIndex].Interact(interactionIndex, itemIndex);
        }

        protected (int, int) CurrentListIndex()
        {
            var listIndex = 0;
            var sizeSum = 0;

            foreach (var list in _lists)
            {
                sizeSum += list.ItemsCount;
                if (_index < sizeSum)
                {
                    return (listIndex, _index - sizeSum + list.ItemsCount);
                }

                listIndex++;
            }

            return (0, 0);
        }
    }
}
