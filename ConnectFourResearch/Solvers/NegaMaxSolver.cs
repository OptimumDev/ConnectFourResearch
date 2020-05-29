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
        private readonly int maxDepth;

        public event Action<int, Countdown> OnDepthHandled;
        public event Action<int> OnStateEvaluated;

        public NegaMaxSolver(Cell me, int maxDepth = Board.Width * Board.Height)
        {
            this.me = me;
            this.maxDepth = maxDepth;
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
            foreach (var possibleMove in problem.GetPossibleMoves())
            {
                var nextState = problem.Move(possibleMove, me);
                var score = -NegaMax(nextState, me.GetOpponent(), depth, countdown, depth);
                yield return new Move(possibleMove, score);
            }
        }

        private double NegaMax(Board gameState, Cell player, int depth, Countdown countdown, int initDepth)
        {
            OnStateEvaluated?.Invoke(initDepth - depth);

            if (depth == 0 || gameState.IsFinished() || countdown.IsFinished())
                return GetEstimateScore(gameState, player);

            var value = double.NegativeInfinity;
            foreach (var possibleMove in gameState.GetPossibleMoves())
            {
                var child = gameState.Move(possibleMove, player);
                value = Math.Max(value, -NegaMax(child, player.GetOpponent(), depth - 1, countdown, initDepth));
            }
            return value;
        }

        private static double GetEstimateScore(Board board, Cell player) =>
            GetPlayerEstimateScore(board, player) - GetPlayerEstimateScore(board, player.GetOpponent());

        private static double GetPlayerEstimateScore(Board board, Cell player) =>
            board.GetLinesCountOfLength(4, player) * 100000 +
            board.GetLinesCountOfLength(3, player) * 1000 +
            board.GetLinesCountOfLength(2, player) * 100;

        public string Name => "NegaMax";
    }
}