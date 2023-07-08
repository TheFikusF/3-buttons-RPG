namespace RPG.Items
{
    public class ItemUseResult
    {
        public readonly string Description;

        public ItemUseResult() : this(string.Empty)
        {

        }

        public ItemUseResult(string description)
        {
            Description = description;
        }
    }
}
