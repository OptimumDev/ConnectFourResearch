namespace ConnectFourResearch.ConnectFour
{
    public class CacheValue
    {
        public readonly double Score;
        public readonly int Depth;
        public readonly CacheType Type;

        public CacheValue(double score, int depth, CacheType type)
        {
            Score = score;
            Depth = depth;
            Type = type;
        }
    }
}