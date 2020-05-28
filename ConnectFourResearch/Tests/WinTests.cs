using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConnectFourResearch.ConnectFour;
using NUnit.Framework;

namespace ConnectFourResearch.Tests
{
    [TestFixture]
    public class WinTests
    {
        [Test]
        public void AllVsAllTest()
        {
            var solverFabrics = TestHelper.GetSolverFabrics().ToList();
            var log = CreateLog(solverFabrics);

            foreach (var yellowSolverFabric in solverFabrics)
            {
                log.Append($"\n| {GetSolverName(yellowSolverFabric)} |");
                foreach (var redSolverFabric in solverFabrics)
                {
                    var result = Play(yellowSolverFabric, redSolverFabric);
                    log.Append($" {CellToResult(result)} |");
                }
            }

            Console.WriteLine(log);
        }

        private static Cell Play(
            TestHelper.SolverFabric yellowSolverFabric,
            TestHelper.SolverFabric redSolverFabric)
        {
            const int maxDepth = Board.Width * Board.Height;
            var yellowSolver = yellowSolverFabric(Cell.Yellow, maxDepth);
            var redSolver = redSolverFabric(Cell.Red, maxDepth);
            var controller = new Controller(yellowSolver, redSolver, 100, null);

            var result = controller.Play();
            if (TestHelper.IsWon(result, Cell.Yellow))
                return Cell.Yellow;
            if (TestHelper.IsWon(result, Cell.Red))
                return Cell.Red;
            return Cell.Empty;
        }

        private static StringBuilder CreateLog(List<TestHelper.SolverFabric> solverFabrics)
        {
            var log = new StringBuilder("Желтый \\ Красный |");
            foreach (var fabric in solverFabrics)
                log.Append($" {GetSolverName(fabric)} |");
            log.Append("\n|");
            for (var i = 0; i < solverFabrics.Count + 1; i++)
                log.Append(" --- |");
            return log;
        }

        private static string CellToResult(Cell result) => result switch
        {
            Cell.Yellow => "Желтый",
            Cell.Red => "Красный",
            Cell.Empty => "Ничья",
            _ => throw new ArgumentException()
        };

        private static string GetSolverName(TestHelper.SolverFabric fabric)
        {
            return fabric(Cell.Yellow, 0).Name;
        }
    }
}