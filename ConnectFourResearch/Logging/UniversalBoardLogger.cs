using System;
using System.Collections.Generic;
using System.Linq;
using ConnectFourResearch.ConnectFour;
using ConnectFourResearch.Extensions;

namespace ConnectFourResearch.Logging
{
    public class UniversalBoardLogger : IBoardLogger
    {
        private static readonly string TopLine = GetBoardBoxLine('┌', '┬', '┐');
        private static readonly string MiddleLine = GetBoardBoxLine('├', '┼', '┤');
        private static readonly string BottomLine = GetBoardBoxLine('└', '┴', '┘');


        private readonly Action<string> logAction;
        private readonly Dictionary<Cell, Action<string>> colorLogActions;

        protected UniversalBoardLogger(Action<string> logAction,
            Action<string> logYellowAction, Action<string> logRedAction)
        {
            this.logAction = logAction;

            colorLogActions = new Dictionary<Cell, Action<string>>
            {
                [Cell.Yellow] = logYellowAction,
                [Cell.Red] = logRedAction,
                [Cell.Empty] = logAction
            };
        }

        public void Log(Board board, IEnumerable<Move> moveVariants, Cell player)
        {
            var isBest = true;

            logAction($"{player} player's turn!\nMove variants:\n");
            foreach (var move in moveVariants)
            {
                logAction($"\t{MoveToString(move)}");
                if (isBest)
                    logAction(" (Best)");
                logAction("\n");

                isBest = false;
            }

            LogBoard(board);
            logAction("\n");
        }

        private static string MoveToString(Move move) => $"{move.Column} : {move.Score:F}";

        public void LogBoard(Board board)
        {
            logAction(TopLine);
            for (var y = Board.Height - 1; y >= 0; y--)
            {
                logAction("│");
                for (var x = 0; x < Board.Width; x++)
                {
                    var cell = board.GetCell(x, y);
                    logAction(" ");
                    colorLogActions[cell]($"{CellToString(cell)}");
                    logAction(" │");
                }

                logAction("\n");
            }

            logAction(MiddleLine);
        }

        public void PrintMoves(HashSet<int> moves)
        {
            logAction("│");
            for (var i = 0; i < Board.Width; i++)
                logAction($" {(moves.Contains(i) ? (i + 1).ToString() : " ")} │");
            logAction($"\n{BottomLine}");
        }

        private static string CellToString(Cell cell) => cell switch
        {
            Cell.Red => "●",
            Cell.Yellow => "●",
            Cell.Empty => " ",
            _ => throw new ArgumentOutOfRangeException(nameof(cell), cell, null)
        };

        private static string GetBoardBoxLine(char startChar, char middleChar, char endChar)
        {
            var middle = Enumerable.Repeat($"───{middleChar}", Board.Width - 1).StrJoin();
            return $"{startChar}{middle}───{endChar}\n";
        }
    }
}