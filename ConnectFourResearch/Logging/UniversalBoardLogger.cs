using System;
using System.Collections.Generic;
using System.Linq;
using ConnectFourResearch.ConnectFour;
using ConnectFourResearch.Extensions;

namespace ConnectFourResearch.Logging
{
    public class UniversalBoardLogger : IBoardLogger
    {
        private static readonly string LineDivider =
            $"|{Enumerable.Repeat("---|", Board.Width).StrJoin()}\n";

        private readonly Action<string> logAction;
        private readonly Dictionary<Cell, Action<string>> colorLogActions;

        public UniversalBoardLogger(Action<string> logAction,
            Action<string> logYellowAction, Action<string> logRedAction)
        {
            this.logAction = logAction;

            colorLogActions = new Dictionary<Cell, Action<string>>
            {
                [Cell.Yellow] = logYellowAction,
                [Cell.Red] = logRedAction
            };
        }

        public void Log(Board board, IEnumerable<Move> moveVariants, Cell player)
        {
            var isBest = true;

            colorLogActions[player](player.ToString());
            logAction(" player's turn!\nMove variants:\n");
            foreach (var move in moveVariants)
            {
                var logMoveAction = isBest ? colorLogActions[player] : logAction;

                logMoveAction($"\t{MoveToString(move)}\n");

                isBest = false;
            }

            LogBoard(board);
            logAction("\n");
        }

        private static string MoveToString(Move move) => $"{move.Column} : {move.Score}";

        private void LogBoard(Board board)
        {
            logAction(LineDivider);
            for (var y = 0; y < Board.Height; y++)
            {
                logAction("|");
                for (var x = 0; x < Board.Width; x++)
                {
                    var cell = board.GetCell(x, y);
                    logAction(" ");
                    colorLogActions[cell]($"{CellToString(cell)}");
                    logAction(" |\n");
                }
            }

            logAction(LineDivider);
        }

        private static string CellToString(Cell cell) => cell switch
        {
            Cell.Red => "●",
            Cell.Yellow => "●",
            Cell.Empty => " ",
            _ => throw new ArgumentOutOfRangeException(nameof(cell), cell, null)
        };
    }
}