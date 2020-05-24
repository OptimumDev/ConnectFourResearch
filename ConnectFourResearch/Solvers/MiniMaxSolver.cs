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
        private readonly int defaultDepth = 5;
        private bool withCache;

        public MiniMaxSolver(Cell maximazingPlayer, bool withCache = false)
        {
            this.maximazingPlayer = maximazingPlayer;
            this.withCache = withCache;
        }

        public IEnumerable<Move> GetSolutions(Board problem, Countdown countdown)
        {
            return MiniMax(problem, maximazingPlayer, defaultDepth, double.NegativeInfinity, double.PositiveInfinity)
                .OrderBy(m => m.Score);
        }

        private IEnumerable<Move> MiniMax(Board gameState, Cell player, int depth, double alpha, double beta)
        {
            foreach(var move in gameState.GetPossibleMoves())
            {
                var newState = gameState.Move(move, player);
                var score = GetScore(newState, player.GetOpponent(), depth, alpha, beta);
                
                if (maximazingPlayer == player)
                    alpha = Math.Max(alpha, score);
                else
                    beta = Math.Min(beta, score);
                yield return new Move(move, score);
                
                if (alpha >= beta)
                    yield break;
            }
        }
        
        private double GetScore(Board state, Cell player, int depth, double alpha, double beta)
        {
            if (state.IsFinished() || depth == 0)
                return GetEstimateScore(state);

            var moves = MiniMax(state, player, depth - 1, alpha, beta);
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