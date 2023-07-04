using RPG.Entities;

namespace RPG.GameStates
{
    public class SkillsChooserScreen : ListGameState
    {
        public SkillsChooserScreen(Player player, GameState stateToExit) : base(player, stateToExit)
        {
            var list1 = new ItemsList(new List<LabeledListStateItem>()
            {
                new LabeledListStateItem("Main skill", player.Actions.EquippedSpells[0]),
                new LabeledListStateItem("God's skill", player.Actions.EquippedSpells[1]),
            }, "Equipped spells");

            SetupList(list1, new List<ListStateItemInteraction>()
            {
                new ListStateItemInteraction("", (item, index) => { }),

                new ListStateItemInteraction("", (item, index) => { }),
            });

            var list2 = new ItemsList(player.Actions.Spells.Where(x => !x.Item2).Select(x => x.Item1), "Available spells");

            SetupList(list2, new List<ListStateItemInteraction>()
            {
                new ListStateItemInteraction("Equip", (item, index) =>
                {
                    player.Actions.EquipSpell(item as EntityActions.Spell, 0);

                    list1.SetItems(new List<LabeledListStateItem>()
                    {
                        new LabeledListStateItem("Main skill", player.Actions.EquippedSpells[0]),
                        new LabeledListStateItem("God's skill", player.Actions.EquippedSpells[1]),
                    });

                    list2.SetItems(player.Actions.Spells.Where(x => !x.Item2).Select(x => x.Item1));
                }),

                new ListStateItemInteraction("", (item, index) => { }),
            });
        }
    }
}
