using System.Collections.Generic;
using System.Linq;
using ConnectFourResearch.Algorithms;
using ConnectFourResearch.Logging;

namespace ConnectFourResearch.ConnectFour
{
    public class Controller
    {
        private readonly Dictionary<Cell, ISolver<Board, Move>> solvers;
        private readonly IBoardLogger logger;

        public Controller(ISolver<Board, Move> yellowPlayer, ISolver<Board, Move> redPlayer, IBoardLogger logger)
        {
            this.logger = logger;

            solvers = new Dictionary<Cell, ISolver<Board, Move>>
            {
                [Cell.Yellow] = yellowPlayer,
                [Cell.Red] = redPlayer
            };
        }

        public Board Play(bool makeLog = false)
        {
            var board = new Board();
            var currentPlayer = Cell.Yellow;

            while (!board.IsFinished())
            {
                var moves = solvers[currentPlayer].GetSolutions(board, 100).ToList();
                board = board.Move(moves[^1].Column, currentPlayer);
                if (makeLog)
                    logger.Log(board, moves.Skip(moves.Count - 5).Reverse(), currentPlayer);
                currentPlayer = currentPlayer.GetOpponent();
            }

            return board;
        }
    }
}