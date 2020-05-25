using System;
using System.Collections.Generic;
using System.Linq;
using ConnectFourResearch.Algorithms;
using ConnectFourResearch.ConnectFour;

namespace ConnectFourResearch.Solvers
{
    public class NegaMaxSolver : ISolver<Board, Move>
    {
        private readonly Cell me;

        public NegaMaxSolver(Cell me)
        {
            this.me = me;
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
            foreach (var possibleMove in problem.GetPossibleMoves())
            {
                var nextState = problem.Move(possibleMove, me);
                var score = -NegaMax(nextState, me.GetOpponent(), depth);
                yield return new Move(possibleMove, score);
            }
        }

        private double NegaMax(Board gameState, Cell player, int depth)
        {
            if (depth == 0 || gameState.IsFinished())
                return GetEstimateScore(gameState, player);

            var value = double.NegativeInfinity;
            foreach (var possibleMove in gameState.GetPossibleMoves())
            {
                var child = gameState.Move(possibleMove, player);
                value = Math.Max(value, -NegaMax(child, player.GetOpponent(), depth - 1));
            }
            return value;
        }

        private static double GetEstimateScore(Board board, Cell player) =>
            GetPlayerEstimateScore(board, player) - GetPlayerEstimateScore(board, player.GetOpponent());

        private static double GetPlayerEstimateScore(Board board, Cell player) =>
            board.GetLinesCountOfLength(4, player) * 100000 +
            board.GetLinesCountOfLength(3, player) * 1000 +
            board.GetLinesCountOfLength(2, player) * 100;
    }
}