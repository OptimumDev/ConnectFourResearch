using System;
using System.Collections.Generic;
using System.Linq;
using ConnectFourResearch.Algorithms;
using ConnectFourResearch.ConnectFour;

namespace ConnectFourResearch.Solvers
{
    public class MiniMaxSolver : ISolver<Board, Move>
    {
        private readonly Cell maximazingPlayer;
        private readonly bool withCache;
        private readonly int maxDepth;
        private readonly bool withSorting;
        private readonly Dictionary<Board, CacheValue> cache;

        public event Action<int, Countdown> OnDepthHandled;
        public event Action<int> OnStateEvaluated;

        public MiniMaxSolver(Cell maximazingPlayer, bool withSorting = false, bool withCache = false,
            int maxDepth = Board.Width * Board.Height)
        {
            this.maximazingPlayer = maximazingPlayer;
            this.withSorting = withSorting;
            this.withCache = withCache;
            this.maxDepth = maxDepth;
            cache = new Dictionary<Board, CacheValue>();
        }

        public IEnumerable<Move> GetSolutions(Board problem, Countdown countdown)
        {
            IEnumerable<Move> result = new Move[0];

            var depth = 1;
            do
            {
                var nextResult = Solve(problem, depth, countdown).ToList();
                if (!countdown.IsFinished())
                {
                    result = nextResult;
                    OnDepthHandled?.Invoke(depth, countdown);
                }
                depth++;
            } while (!countdown.IsFinished() && depth <= maxDepth);

            return result.OrderBy(m => m.Score);
        }

        private IEnumerable<Move> Solve(Board problem, int depth, Countdown countdown)
        {
            var moves = problem.GetPossibleMoves();
            if (withSorting)
                moves = SortPossibleMoves(moves);

            return moves.Select(m => EvaluateMove(problem, m, depth, countdown));
        }


        private Move EvaluateMove(Board gameState, int move, int depth, Countdown countdown)
        {
            var nextState = gameState.Move(move, maximazingPlayer);
            var score = MiniMax(nextState, maximazingPlayer.GetOpponent(), depth, countdown, depth);
            return new Move(move, score);
        }

        private double MiniMax(Board gameState, Cell player, int depth, Countdown countdown, int initDepth,
            double alpha = double.NegativeInfinity, double beta = double.PositiveInfinity)
        {
            OnStateEvaluated?.Invoke(initDepth - depth);
            if (gameState.IsFinished() || depth == 0 || countdown.IsFinished())
                return GetEstimateScore(gameState);

            if (withCache && TryGetCachedScore(gameState, depth, ref alpha, ref beta, out var cachedScore))
                return cachedScore;

            return player == maximazingPlayer
                ? MaximizeScore(gameState, player, depth, countdown, initDepth, alpha, beta)
                : MinimizeScore(gameState, player, depth, countdown, initDepth, alpha, beta);
        }

        private bool TryGetCachedScore(Board gameState, int depth, ref double alpha, ref double beta,
            out double cachedScore)
        {
            cachedScore = double.NegativeInfinity;

            if (!cache.TryGetValue(gameState, out var cached) || cached.Depth < depth)
                return false;
            switch (cached.Type)
            {
                case CacheType.Exact:
                    cachedScore = cached.Score;
                    return true;
                case CacheType.LowerBound:
                    if (cached.Score >= beta)
                    {
                        cachedScore = cached.Score;
                        return true;
                    }
                    alpha = Math.Max(alpha, cached.Score);
                    break;
                case CacheType.UpperBound:
                    if (cached.Score <= alpha)
                    {
                        cachedScore = cached.Score;
                        return true;
                    }
                    beta = Math.Min(beta, cached.Score);
                    break;
            }

            return false;
        }

        private double MinimizeScore(Board gameState, Cell player, int depth, Countdown countdown,
            int initDepth, double alpha, double beta)
        {
            var type = CacheType.LowerBound;
            var score = double.PositiveInfinity;
            foreach (var move in gameState.GetPossibleMoves())
            {
                var nextState = gameState.Move(move, player);
                var nextStateScore = MiniMax(nextState, player.GetOpponent(), depth - 1,
                    countdown, initDepth, alpha, beta);
                score = Math.Min(score, nextStateScore);
                if (score < beta)
                {
                    beta = score;
                    type = CacheType.Exact;
                }
                if (alpha >= beta)
                {
                    type = CacheType.UpperBound;
                    break;
                }
            }

            if (withCache)
                cache[gameState] = new CacheValue(score, depth, type);
            return score;
        }

        private double MaximizeScore(Board gameState, Cell player, int depth, Countdown countdown,
            int initDepth, double alpha, double beta)
        {
            var type = CacheType.UpperBound;
            var score = double.NegativeInfinity;
            foreach (var move in gameState.GetPossibleMoves())
            {
                var nextState = gameState.Move(move, player);
                var nextStateScore = MiniMax(nextState, player.GetOpponent(), depth - 1,
                    countdown, initDepth, alpha, beta);
                score = Math.Max(score, nextStateScore);
                if (score > alpha)
                {
                    alpha = score;
                    type = CacheType.Exact;
                }
                if (alpha >= beta)
                {
                    type = CacheType.LowerBound;
                    break;
                }
            }

            if (withCache)
                cache[gameState] = new CacheValue(score, depth, type);
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

        public string Name => $"MiniMax{(withSorting ? " with sorting" : "")}{(withCache ? " with cache" : "")}";
    }
}