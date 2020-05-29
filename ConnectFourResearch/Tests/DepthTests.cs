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
    public class DepthTests
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

        [Test]
        public void CountAverageDepthStatesCount()
        {
            var fabrics = TestHelper.GetSolverFabrics().ToList();
            var log = CreateLog(fabrics);

            var results = fabrics
                .Select(RunDepthStatesTest)
                .ToList();

            for (var i = 1; i <= 42; i++)
            {
                log.Append($"\n| {i} |");
                foreach (var result in results)
                    log.Append($" {result.GetAverageDepthStatesStr(i)} |");
            }

            Console.WriteLine(log);
        }

        private static DepthTimeTestStat RunDepthTimeTest(TestHelper.SolverFabric solverFabric)
        {
            var yellowSolver = solverFabric(Cell.Yellow);

            var stat = new DepthTimeTestStat();
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

        private static DepthStatesTestStat RunDepthStatesTest(TestHelper.SolverFabric solverFabric)
        {
            var yellowSolver = solverFabric(Cell.Yellow);

            var stat = new DepthStatesTestStat();
            var statesCount = new Dictionary<int, int>();
            AddDepthStatesHandlers(statesCount, stat, yellowSolver);

            foreach (var fabric in TestHelper.GetSolverFabrics())
            {
                var redSolver = fabric(Cell.Red);
                var controller = new Controller(yellowSolver, redSolver, 300, null);
                controller.Play();
            }

            return stat;
        }

        private static void AddDepthStatesHandlers(Dictionary<int, int> statesCount, DepthStatesTestStat stat, ISolver<Board, Move> yellowSolver)
        {
            void IncStates(int depth)
            {
                if (!statesCount.ContainsKey(depth))
                    statesCount[depth] = 0;
                statesCount[depth]++;
            }

            void AddStat(int depth, Countdown countdown)
            {
                foreach (var pair in statesCount)
                    stat.AddDepthState(pair.Key, pair.Value);
                statesCount.Clear();
            }

            yellowSolver.OnStateEvaluated += IncStates;
            yellowSolver.OnDepthHandled += AddStat;
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