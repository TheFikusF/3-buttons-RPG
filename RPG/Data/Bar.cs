namespace RPG.Data
{
    public class Bar
    {
        private int _minValue;
        private int _maxValue;
        private int _value;

        public int MaxValue
        {
            get => _maxValue;
            set
            {
                if(value < MinValue)
                {
                    return;
                }

                if(_value > value)
                {
                    _value = value;
                }

                _maxValue = value;
            }
        }

        public int MinValue
        {
            get => _minValue;
            set
            {
                if (value > MaxValue)
                {
                    return;
                }

                if (_value < value)
                {
                    _value = value;
                }

                _minValue = value;
            }
        }

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                if (_value < MinValue)
                {
                    _value = MinValue;
                }

                if (_value > MaxValue)
                {
                    _value = MaxValue;
                }
            }
        }

        public Bar(int minValue, int maxValue, int value)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            Value = value;
        }
    }
}
