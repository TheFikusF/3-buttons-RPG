using RPG.Items;

namespace RPG.Fight.ActionResults
{
    public interface IActionResult
    {
        public List<ItemUseResult> ItemUseResults { get; }

        public string ToString();
    }
}
