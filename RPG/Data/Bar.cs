namespace RPG.Data
{
    public class Bar
    {
        private int _minValue;
        private int _maxValue;
        private int _value;

        private Func<int, int> _getMaxValue;
        private Func<int, int> _getMinValue;

        public int MaxValue
        {
            get => _getMaxValue(_maxValue);
            set
            {
                if(value < MinValue)
                {
                    return;
                }

                _value = _value > value ? value : _value;
                
                _maxValue = value;
            }
        }

        public int MinValue
        {
            get => _getMinValue(_minValue);
            set
            {
                if (value > MaxValue)
                {
                    return;
                }

                _value = _value < value ? value : _value;

                _minValue = value;
            }
        }

        public int Value
        {
            get => _value;
            set => _value = value < MinValue ? MinValue : (value > MaxValue ? MaxValue : value);
        }

        public Bar(int minValue, int maxValue, int value, Func<int, int> getMinValue = null, Func<int, int> getMaxValue = null)
        {
            _getMaxValue = getMaxValue is null ? ((x) => x) : getMaxValue;
            _getMinValue = getMinValue is null ? ((x) => x) : getMinValue;

            MinValue = minValue;
            MaxValue = maxValue;
            Value = value;
        }
    }
}
