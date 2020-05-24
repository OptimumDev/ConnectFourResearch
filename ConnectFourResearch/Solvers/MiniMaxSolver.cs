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
        private bool withCache;
        private Dictionary<Board, double> cache = new Dictionary<Board, double>();

        public MiniMaxSolver(Cell maximazingPlayer, bool withCache = false)
        {
            this.maximazingPlayer = maximazingPlayer;
            this.withCache = withCache;
        }

        public IEnumerable<Move> GetSolutions(Board problem, Countdown countdown)
        {
            IEnumerable<Move> result;
            
            var depth = 1;
            do
            {
                // А нужно ли теперь Countdown прокидывать?
                result = MiniMax(problem, maximazingPlayer, depth, double.NegativeInfinity, double.PositiveInfinity, countdown);
                depth++;
            } while (!countdown.IsFinished());
            
            return result.OrderBy(m => m.Score);
        }

        private IEnumerable<Move> MiniMax(Board gameState, Cell player, int depth, double alpha, double beta, Countdown countdown)
        {
            foreach(var move in gameState.GetPossibleMoves())
            {
                var newState = gameState.Move(move, player);

                // Насколько мне известно, компилятор должен убрать недостижимые ветки
                if (!withCache || !cache.TryGetValue(gameState, out var score))
                {
                    score = GetScore(newState, player.GetOpponent(), depth, alpha, beta, countdown);
                    if (withCache)
                        cache[gameState] = score;
                }
                
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

        // Очень осмысленный метод
        private double GetEstimateScore(Board board) => GetEstimateScore(board, maximazingPlayer);
        
        private static double GetEstimateScore(Board board, Cell player) =>
            board.GetLinesCountOfLength(4, player) * 100000 +
            board.GetLinesCountOfLength(3, player) * 1000 +
            board.GetLinesCountOfLength(2, player) * 100;
    }
}