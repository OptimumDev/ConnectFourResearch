using System.Collections.Generic;
using System.Linq;

namespace ConnectFourResearch.TestStats
{
    public class DepthTimeTestStat
    {
        private readonly Dictionary<int, List<double>> depthTime = new Dictionary<int, List<double>>();

        public void AddDepthTime(int depth, double timeMs)
        {
            if (!depthTime.ContainsKey(depth))
                depthTime[depth] = new List<double>();
            depthTime[depth].Add(timeMs);
        }

        public string GetAverageDepthTimeStr(int depth)
        {
            return depthTime.TryGetValue(depth, out var times)
                ? $"{times.Sum() / times.Count : ##0.0000}"
                : "?";
        }
    }
}