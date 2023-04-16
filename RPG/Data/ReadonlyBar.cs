namespace RPG.Data
{
    public struct ReadonlyBar
    {
        public readonly int Value;
        public readonly int MaxValue;
        public readonly int MinValue;

        public ReadonlyBar(int value, int maxValue, int minValue)
        {
            Value = value;
            MaxValue = maxValue;
            MinValue = minValue;
        }

        public ReadonlyBar(Bar bar) : this(bar.Value, bar.MaxValue, bar.MinValue) { }
    }
}
