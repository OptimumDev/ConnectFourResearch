using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConnectFourResearch.Algorithms;
using ConnectFourResearch.ConnectFour;
using ConnectFourResearch.TestStats;
using NUnit.Framework;

namespace ConnectFourResearch.Tests
{
    [TestFixture]
    public class DepthTimeTests
    {
        [Test]
        public void CountAverageDepthTime()
        {
            var fabrics = TestHelper.GetSolverFabrics().ToList();
            var log = CreateLog(fabrics);

            var results = fabrics
                .Select(RunDepthTimeTest)
                .ToList();

            for (var i = 1; i <= 42; i++)
            {
                log.Append($"\n| {i} |");
                foreach (var result in results)
                    log.Append($" {result.GetAverageDepthTimeStr(i)} |");
            }

            Console.WriteLine(log);
        }

        private static DepthTestStat RunDepthTimeTest(TestHelper.SolverFabric solverFabric)
        {
            var yellowSolver = solverFabric(Cell.Yellow);

            var stat = new DepthTestStat(yellowSolver.Name);
            void AddStat(int depth, Countdown countdown)
            {
                stat.AddDepthTime(depth, countdown.TimeElapsed.TotalMilliseconds);
            }
            yellowSolver.OnDepthHandled += AddStat;

            foreach (var fabric in TestHelper.GetSolverFabrics())
            {
                var redSolver = fabric(Cell.Red);
                var controller = new Controller(yellowSolver, redSolver, 300, null);
                controller.Play();
            }

            return stat;
        }

        private static StringBuilder CreateLog(List<TestHelper.SolverFabric> fabrics)
        {
            var log = new StringBuilder("| Depth |");
            foreach (var fabric in fabrics)
                log.Append($" {TestHelper.GetSolverName(fabric)} |");
            log.Append("\n| :---: |");
            for (var i = 0; i < fabrics.Count; i++)
                log.Append(" --- |");
            return log;
        }
    }
}