using System.Collections.Generic;
using System.Linq;
using ConnectFourResearch.Algorithms;
using ConnectFourResearch.ConnectFour;

namespace ConnectFourResearch.Solvers
{
    public class NegaMaxSolver : ISolver<Board, Move>
    {
        private readonly Cell me;
        private readonly int defaultDepth = 5;

        public NegaMaxSolver(Cell me)
        {
            this.me = me;
        }

        public IEnumerable<Move> GetSolutions(Board problem, Countdown countdown)
        {
            return Solve(problem, me, defaultDepth, countdown).OrderBy(m => m.Score);
        }

        private IEnumerable<Move> Solve(Board problem, Cell player, int depth, Countdown countdown)
        {
            return problem
                .GetPossibleMoves()
                .Select(move => new Move(move, -GetScore(problem.Move(move, player), player.GetOpponent(), depth, countdown)));
        }

        private double GetScore(Board board, Cell player, int depth, Countdown countdown) => 
            (board.IsFinished(), depth, countdown.IsFinished()) switch
        {
            (true, _, _) => GetFinishScore(board, player),
            (_, 0, _) => GetEstimateScore(board, player),
            (_, _, true) => GetEstimateScore(board, player),
            _ => Solve(board, player, depth - 1, countdown).Max(b => b.Score)
        };

        private static double GetFinishScore(Board board, Cell player) =>
            (board.GetLinesCountOfLength(4, player) -
            board.GetLinesCountOfLength(4, player.GetOpponent())) *
            1_000_000_000;

        private static double GetEstimateScore(Board board, Cell player) =>
            GetPlayerEstimateScore(board, player) - GetPlayerEstimateScore(board, player.GetOpponent());

        // Enumerable.Range(2, 3).Select(i => board.GetLinesCountOfLength(i, player) * Math.Pow(10, i)).Sum();
        private static double GetPlayerEstimateScore(Board board, Cell player) =>
            board.GetLinesCountOfLength(4, player) * 100000 +
            board.GetLinesCountOfLength(3, player) * 1000 +
            board.GetLinesCountOfLength(2, player) * 100;
    }
}