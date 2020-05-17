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
            Console.Clear();
            logger.LogBoard(problem);
            Console.Write("|");
            for (var i = 0; i < Board.Width; i++)
                Console.Write($" {i + 1} |");
            Console.WriteLine();

            Console.WriteLine("Choose column to place disk (Use number keys)");
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey();
            } while (!char.IsDigit(key.KeyChar));
            Console.WriteLine("\n");
            yield return new Move(int.Parse(key.KeyChar.ToString()) - 1, 0);
        }
    }
}