using RPG.Entities;

namespace RPG.GameStates
{
    public class MarketScreen : ListGameState
    {
        private class SellableItem : IListStateItem
        {
            public string GetFullString(int pad)
            {
                return "workaet :)";
            }
        }

        public MarketScreen(Player player, GameState stateToExit) : base(player, stateToExit)
        {
            var list1 = new ItemsList(new List<SellableItem>()
            {
                new SellableItem(),
                new SellableItem(),
                new SellableItem(),
                new SellableItem(),
            },
            "Market1");

            SetupList(list1, new List<ListStateItemInteraction>()
            {
                new ListStateItemInteraction("Remove", (item, index) =>
                {
                    list1.Items.Remove(item);
                }),

                new ListStateItemInteraction("Delete", (item, index) =>
                {
                    list1.Items.Remove(item);
                }),
            });

            var list2 = new ItemsList(new List<LabeledListStateItem>()
            {
                new LabeledListStateItem("first", new SellableItem()),
                new LabeledListStateItem("second", new SellableItem()),
                new LabeledListStateItem("third", new SellableItem()),
                new LabeledListStateItem("fourth", new SellableItem()),
            },
            "Market2");

            SetupList(list2, new List<ListStateItemInteraction>()
            {
                new ListStateItemInteraction("Kill", (item, index) =>
                {
                    list2.Items.RemoveAt(index);
                }),

                new ListStateItemInteraction("Destroy", (item, index) =>
                {
                    list2.Items.RemoveAt(index);
                }),
            });
        }
    }
}
