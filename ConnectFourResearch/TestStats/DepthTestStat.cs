using System.Collections.Generic;
using System.Linq;

namespace ConnectFourResearch.TestStats
{
    public class DepthTestStat
    {
        public readonly Dictionary<int, List<double>> DepthTime = new Dictionary<int, List<double>>();
        public readonly string SolverName;

        public DepthTestStat(string solverName)
        {
            SolverName = solverName;
        }

        public void AddDepthTime(int depth, double timeMs)
        {
            if (!DepthTime.ContainsKey(depth))
                DepthTime[depth] = new List<double>();
            DepthTime[depth].Add(timeMs);
        }

        public string GetAverageDepthTimeStr(int depth)
        {
            return DepthTime.TryGetValue(depth, out var times)
                ? $"{times.Sum() / times.Count : ##0.0000}"
                : "?";
        }
    }
}