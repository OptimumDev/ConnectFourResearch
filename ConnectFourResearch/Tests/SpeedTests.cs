using System;
using System.Collections.Generic;
using System.Linq;
using ConnectFourResearch.Algorithms;
using ConnectFourResearch.ConnectFour;
using ConnectFourResearch.Solvers;
using NUnit.Framework;

namespace ConnectFourResearch.Tests
{
    [TestFixture]
    public class SpeedTests
    {
        private const int TestTime = 10_000;

        private delegate ISolver<Board, Move> SolverFabric(Cell player, int maxDepth);

        [Test]
        public void SpeedOfAllTest()
        {
            var results = GetSolverFabrics()
                .Select(RunSpeedTest)
                .OrderByDescending(s => s.Score);

            Console.WriteLine("| Solver | Games | Wins | Score |");
            foreach (var result in results)
                Console.WriteLine(result);
        }

        // Насколько быстро выигрываем жадный алгоритм
        private static TestStat RunSpeedTest(SolverFabric fabric)
        {
            var solver = fabric(Cell.Yellow, 5);
            var greedy = new GreedySolver(Cell.Red);
            var controller = new Controller(solver, greedy, 100, null);

            var gamesCount = 0;
            var winsCount = 0;

            var countDown = new Countdown(TestTime);
            while (!countDown.IsFinished())
            {
                var result = controller.Play();
                gamesCount++;
                if (IsWon(result, Cell.Yellow))
                    winsCount++;
                if (IsWon(result, Cell.Red))
                    return null;
            }

            return new TestStat(gamesCount, winsCount, solver.Name);
        }

        private static bool IsWon(Board game, Cell player) => game.GetLinesCountOfLength(4, player) > 0;

        private static IEnumerable<SolverFabric> GetSolverFabrics()
        {
            yield return (c, d) => new NegaMaxSolver(c, d);
            yield return (c, d) => new MiniMaxSolver(c, maxDepth: d);
            yield return (c, d) => new MiniMaxSolver(c, true, maxDepth: d);
            yield return (c, d) => new MiniMaxSolver(c, false, true, d);
            yield return (c, d) => new MiniMaxSolver(c, true, true, d);
        }
    }
}