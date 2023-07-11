using RPG.Fight.ActionResults;

namespace RPG.Items
{
    public class ItemUseResult : IActionResult
    {
        public readonly string Description;
        public List<ItemUseResult> ItemUseResults { get; private set; }

        public ItemUseResult() : this(string.Empty)
        {

        }

        public ItemUseResult(string description)
        {
            Description = description;
            ItemUseResults = new List<ItemUseResult>();
        }

        public override string ToString()
        {
            return Description + 
                (ItemUseResults.Count > 0 
                ? (Environment.NewLine + string.Join(Environment.NewLine, ItemUseResults)) 
                : string.Empty);
        }
    }
}
