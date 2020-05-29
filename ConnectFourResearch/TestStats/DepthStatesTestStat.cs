using System.Collections.Generic;
using System.Linq;

namespace ConnectFourResearch.TestStats
{
    public class DepthStatesTestStat
    {
        private readonly Dictionary<int, List<int>> depthStates = new Dictionary<int, List<int>>();

        public void AddDepthState(int depth, int states)
        {
            if (!depthStates.ContainsKey(depth))
                depthStates[depth] = new List<int>();
            depthStates[depth].Add(states);
        }

        public string GetAverageDepthStatesStr(int depth)
        {
            return depthStates.TryGetValue(depth, out var times)
                ? $"{times.Sum() / times.Count}"
                : "?";
        }
    }
}