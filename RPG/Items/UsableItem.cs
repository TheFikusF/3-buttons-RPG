namespace RPG.Items
{
    public class UsableItem : Item
    {
        public UsableItem(string name) : base(name)
        {
        }

        public void UseItemCombat()
        {

        }

        public void UseItemIdle()
        {

        }

        public override string GetFullString()
        {
            throw new NotImplementedException();
        }

        public override string GetFullString(int pad)
        {
            throw new NotImplementedException();
        }
    }
}
