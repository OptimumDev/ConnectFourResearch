using System;
using System.Collections.Generic;
using ConnectFourResearch.Algorithms;
using ConnectFourResearch.ConnectFour;
using ConnectFourResearch.Logging;

namespace ConnectFourResearch.Solvers
{
    public class ConsoleSolver : ISolver<Board, Move>
    {
        private readonly ConsoleBoardLogger logger = new ConsoleBoardLogger();

        public IEnumerable<Move> GetSolutions(Board problem, Countdown countdown)
        {
            var moves = new HashSet<int>(problem.GetPossibleMoves());

            Console.Clear();
            logger.LogBoard(problem);
            logger.PrintMoves(moves);
            Console.WriteLine("Choose column to place disk (Use number keys)");
            var column = GetColumn(moves);
            Console.WriteLine("\n");

            yield return new Move(column, 0);
        }

        private static int GetColumn(HashSet<int> moves)
        {
            int column;
            bool isDigit;
            do
            {
                Console.Write("\r");
                var key = Console.ReadKey();
                isDigit = int.TryParse(key.KeyChar.ToString(), out column);
            } while (!isDigit || !moves.Contains(column - 1));

            return column - 1;
        }

        public string Name => "Console solver for human player";

        public event Action<int, Countdown> OnDepthHandled;
        public event Action<int> OnStateEvaluated;
    }
}