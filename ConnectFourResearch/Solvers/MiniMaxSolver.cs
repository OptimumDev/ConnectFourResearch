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
        private int defaultDepth = 10;
        private bool withCache;
        private Dictionary<Board, Move> cache = new Dictionary<Board, Move>();

        public MiniMaxSolver(Cell maximazingPlayer, bool withCache = false)
        {
            this.maximazingPlayer = maximazingPlayer;
            this.withCache = withCache;
        }

        public IEnumerable<Move> GetSolutions(Board problem, Countdown countdown)
        {
            if (withCache)
                return new [] {CachingMiniMax(problem, maximazingPlayer, defaultDepth, double.NegativeInfinity, double.PositiveInfinity, countdown)};
            
            IEnumerable<Move> result = new Move[0];
            
            var depth = 1;
            do
            {
                // А нужно ли теперь Countdown прокидывать?
                var newResult = MiniMax(problem, maximazingPlayer, depth, double.NegativeInfinity, double.PositiveInfinity, countdown);
                if (!countdown.IsFinished())
                    result = newResult;
                depth++;
            } while (!countdown.IsFinished());
            
            return result.OrderBy(m => m.Score);
        }

        private Move CachingMiniMax(Board gameState, Cell player, int depth, double alpha, double beta, Countdown countdown)
        {
            if (cache.TryGetValue(gameState, out var result))
                return result;

            var moves = MiniMax(gameState, player, depth, alpha, beta, countdown)
                .OrderBy(m => m.Score)
                .Last();

            cache[gameState] = moves;

            return moves;

        }

        private IEnumerable<Move> MiniMax(Board gameState, Cell player, int depth, double alpha, double beta, Countdown countdown)
        {
            foreach(var move in gameState.GetPossibleMoves())
            {
                var newState = gameState.Move(move, player);
                var score = GetScore(newState, player.GetOpponent(), depth, alpha, beta, countdown);
                
                if (maximazingPlayer == player)
                    alpha = Math.Max(alpha, score);
                else
                    beta = Math.Min(beta, score);
                yield return new Move(move, score);
                
                if (alpha >= beta)
                    yield break;
            }
        }
        
        private double GetScore(Board state, Cell player, int depth, double alpha, double beta, Countdown countdown)
        {
            if (state.IsFinished() || depth == 0 || countdown.IsFinished())
                return GetEstimateScore(state);

            var moves = MiniMax(state, player, depth - 1, alpha, beta, countdown);
            var bestMove = player == maximazingPlayer
                ? moves.MaxBy(m => m.Score)
                : moves.MinBy(m => m.Score);
                
            return bestMove.Score;
        }

        private double GetEstimateScore(Board board) => 
            GetEstimateScore(board, maximazingPlayer) - GetEstimateScore(board, maximazingPlayer.GetOpponent());
        
        private static double GetEstimateScore(Board board, Cell player) =>
            board.GetLinesCountOfLength(4, player) * 100000 +
            board.GetLinesCountOfLength(3, player) * 1000 +
            board.GetLinesCountOfLength(2, player) * 100;
    }
}