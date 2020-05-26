using System;
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

        [Test]
        public void SpeedOfAllTest()
        {
            var results = TestHelper.GetSolverFabrics()
                .Select(RunSpeedTest)
                .ToList();

            if (results.Any(r => r == null))
                Assert.Fail("Someone lost((");

            var ordered = results.OrderByDescending(s => s.Score);
            Console.WriteLine("| Solver | Games | Wins | Score |");
            Console.WriteLine("| --- | --- | --- | --- |");
            foreach (var result in ordered)
                Console.WriteLine(result);
        }

        // Насколько быстро выигрываем жадный алгоритм
        private static TestStat RunSpeedTest(TestHelper.SolverFabric fabric)
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
                if (TestHelper.IsWon(result, Cell.Yellow))
                    winsCount++;
                if (TestHelper.IsWon(result, Cell.Red))
                    return null;
            }

            return new TestStat(gamesCount, winsCount, solver.Name);
        }
    }
}