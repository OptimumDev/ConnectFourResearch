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
            return Solve(problem, me, defaultDepth).OrderBy(m => m.Score);
        }
        
        public IEnumerable<Move> Solve(Board problem, Cell player, int depth)
        {
            return problem
                .GetPossibleMoves()
                .Select(move => new Move(move, -GetScore(problem.Move(move, player), player.GetOpponent(), depth)))
                .OrderBy(m => m.Score);
        }

        private double GetScore(Board board, Cell player, int depth) => (board.IsFinished(), depth) switch
        {
            (true, _) => GetFinishScore(board, player),
            (_, 0) => GetEstimateScore(board, player),
            _ => Solve(board, player, depth - 1).Max(b => b.Score)
        };

        private static double GetFinishScore(Board board, Cell player) =>
            board.GetLinesCountOfLength(4, player) -
            board.GetLinesCountOfLength(4, player.GetOpponent()) *
            1_000_000_000;

        private static double GetEstimateScore(Board board, Cell player) =>
            board.GetLinesCountOfLength(4, player) * 100000 +
            board.GetLinesCountOfLength(3, player) * 1000 +
            board.GetLinesCountOfLength(2, player) * 100;
    }
}