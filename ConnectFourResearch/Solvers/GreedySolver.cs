using System.Collections.Generic;
using System.Linq;
using ConnectFourResearch.Algorithms;
using ConnectFourResearch.ConnectFour;
using ConnectFourResearch.Extensions;

namespace ConnectFourResearch.Solvers
{
    public class GreedySolver : ISolver<Board, Move>
    {
        private readonly Cell me;

        public GreedySolver(Cell me)
        {
            this.me = me;
        }
        public IEnumerable<Move> GetSolutions(Board problem, Countdown countdown)
        {
            return GreedySearch(problem, me)
                .OrderBy(m => m.Score);
        }
        
        private IEnumerable<Move> GreedySearch(Board gameState, Cell player)
        {
            foreach(var move in gameState.GetPossibleMoves())
            {
                var newState = gameState.Move(move, player);
                var score = GetEstimateScore(newState);
                
                yield return new Move(move, score);
            }
        }

        private double GetEstimateScore(Board board) =>
            GetEstimateScore(board, me) - GetEstimateScore(board, me.GetOpponent());
        
        private static double GetEstimateScore(Board board, Cell player) =>
            board.GetLinesCountOfLength(4, player) * 100000 +
            board.GetLinesCountOfLength(3, player) * 1000 +
            board.GetLinesCountOfLength(2, player) * 100;

        public string Name => "Greedy";
    }
}