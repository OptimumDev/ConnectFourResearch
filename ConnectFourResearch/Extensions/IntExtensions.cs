namespace ConnectFourResearch.Extensions
{
    public static class IntExtensions
    {
        public static int SetBit(this int x, int bitIndex) => x | (1 << bitIndex);

        public static int GetBit(this int x, int bitIndex) => (x >> bitIndex) & 1;
        
        public static int BoundTo(this int v, int left, int right)
        {
            if (v < left) 
                return left;
            if (v > right) 
                return right;
            return v;
        }
        
        public static int TruncateAbs(this int v, int maxAbs)
        {
            if (v < -maxAbs) 
                return -maxAbs;
            if (v > maxAbs) 
                return maxAbs;
            return v;
        }
        
        public static bool InRange(this int v, int min, int max) => v >= min && v <= max;
    }
}