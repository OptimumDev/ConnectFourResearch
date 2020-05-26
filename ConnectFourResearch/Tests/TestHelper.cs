using System.Collections.Generic;
using ConnectFourResearch.Algorithms;
using ConnectFourResearch.ConnectFour;
using ConnectFourResearch.Solvers;

namespace ConnectFourResearch.Tests
{
    public static class TestHelper
    {
        public delegate ISolver<Board, Move> SolverFabric(Cell player, int maxDepth);

        public static IEnumerable<SolverFabric> GetSolverFabrics()
        {
            yield return (c, d) => new NegaMaxSolver(c, d);
            yield return (c, d) => new MiniMaxSolver(c, maxDepth: d);
            yield return (c, d) => new MiniMaxSolver(c, true, maxDepth: d);
            yield return (c, d) => new MiniMaxSolver(c, false, true, d);
            yield return (c, d) => new MiniMaxSolver(c, true, true, d);
        }

        public static bool IsWon(Board game, Cell player) => game.GetLinesCountOfLength(4, player) > 0;
    }
}