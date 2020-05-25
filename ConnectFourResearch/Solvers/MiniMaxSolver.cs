using System;
using System.Collections.Generic;
using System.Linq;
using ConnectFourResearch.Algorithms;
using ConnectFourResearch.ConnectFour;
using ConnectFourResearch.Extensions;

namespace ConnectFourResearch.Solvers
{
    public class MiniMaxSolver : ISolver<Board, Move>
    {
        private readonly Cell maximazingPlayer;
        private readonly bool withCache;
        private readonly bool withSorting;
        private readonly Dictionary<(Board, int), double> cache;

        public MiniMaxSolver(Cell maximazingPlayer, bool withSorting = true, bool withCache = false)
        {
            this.maximazingPlayer = maximazingPlayer;
            this.withSorting = withSorting;
            this.withCache = withCache;
            cache = new Dictionary<(Board, int), double>();
        }

        public IEnumerable<Move> GetSolutions(Board problem, Countdown countdown)
        {
            IEnumerable<Move> result;

            var depth = 1;
            do
            {
                result = Solve(problem, depth).ToList();
                depth++;
            } while (!countdown.IsFinished());

            Console.WriteLine($"depth: {depth}");
            return result.OrderBy(m => m.Score);
        }

        private IEnumerable<Move> Solve(Board problem, int depth)
        {
            var moves = problem.GetPossibleMoves();
            if (withSorting)
                moves = SortPossibleMoves(moves);

            return moves.Select(m => EvaluateMove(problem, m, depth));
        }


        private Move EvaluateMove(Board gameState, int move, int depth)
        {
            var nextState = gameState.Move(move, maximazingPlayer);
            var score = MiniMax(nextState, maximazingPlayer.GetOpponent(), depth);
            return new Move(move, score);
        }

        private double MiniMax(Board gameState, Cell player, int depth,
            double alpha = double.NegativeInfinity, double beta = double.PositiveInfinity)
        {
            if (gameState.IsFinished() || depth == 0)
                return GetEstimateScore(gameState);

            return player == maximazingPlayer
                ? MaximizeScore(gameState, player, depth, alpha, beta)
                : MinimizeScore(gameState, player, depth, alpha, beta);
        }

        private double MinimizeScore(Board gameState, Cell player, int depth, double alpha, double beta)
        {
            var score = double.PositiveInfinity;
            foreach (var move in gameState.GetPossibleMoves())
            {
                var nextState = gameState.Move(move, player);
                var nextStateScore = MiniMax(nextState, player.GetOpponent(), depth - 1, alpha, beta);
                score = Math.Min(score, nextStateScore);
                beta = Math.Min(beta, score);
                if (alpha >= beta)
                    break;
            }
            return score;
        }

        private double MaximizeScore(Board gameState, Cell player, int depth, double alpha, double beta)
        {
            var score = double.NegativeInfinity;
            foreach (var move in gameState.GetPossibleMoves())
            {
                var nextState = gameState.Move(move, player);
                var nextStateScore = MiniMax(nextState, player.GetOpponent(), depth - 1, alpha, beta);
                score = Math.Max(score, nextStateScore);
                alpha = Math.Max(alpha, score);
                if (alpha >= beta)
                    break;
            }
            return score;
        }

        private double GetEstimateScore(Board board) =>
            GetEstimateScore(board, maximazingPlayer) - GetEstimateScore(board, maximazingPlayer.GetOpponent());

        private static double GetEstimateScore(Board board, Cell player) =>
            board.GetLinesCountOfLength(4, player) * 100000 +
            board.GetLinesCountOfLength(3, player) * 1000 +
            board.GetLinesCountOfLength(2, player) * 100;

        private static IEnumerable<int> SortPossibleMoves(IEnumerable<int> possibleMoves)
        {
            return possibleMoves.OrderBy(m => Math.Abs(m - Board.Width / 2));
        }
    }
}