namespace Unity1week202112.Domain
{
    public readonly struct StatusChange
    {
        public int Before => _before;
        public int After => _after;
        public int Offset => _after - _before;

        private readonly int _before;
        private readonly int _after;

        public StatusChange(int before, int after)
        {
            _before = before;
            _after = after;
        }

        public static StatusChange FromBeforeAndAfter(int before, int after)
        {
            return new StatusChange(before, after);
        }

        public static StatusChange FromBeforeAndAddValue(int before, int addValue)
        {
            return new StatusChange(before, before + addValue);
        }
    }
}
