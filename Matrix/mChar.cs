namespace Matrix
{
    class MChar
    {
        public readonly char Ch;
        public int X { get; set; }
        public int Y { get; set; }
        private int _alpha;
        private int _delay;

        public MChar(char ch, int x, int y, int delay)
        {
            Ch = ch;
            X = x;
            Y = y;
            _delay = delay;
            _alpha = 255;
        }

        public void Tick()
        {
            if (_delay > 0)
            {
                _delay -= 1;
            }
            else if (_alpha > 0)
            {
                _alpha -= 3;
            }
        }

        public int GetAlpha() => _delay <= 0 && _alpha > 0 ? _alpha : 0;

        public bool WasUsed() => _alpha <= 0;
    }
}