using System;
using System.Text;
using ConnectFourResearch.ConnectFour;
using ConnectFourResearch.Logging;
using ConnectFourResearch.Solvers;

namespace ConnectFourResearch
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            var logger = new ConsoleBoardLogger();

            var yellowPlayer = new MiniMaxSolver(Cell.Yellow, true);
            var redPlayer = new NegaMaxSolver(Cell.Red);
            var controller = new Controller(yellowPlayer, redPlayer, logger);

            var result = controller.Play(true);

            Console.Clear();
            logger.LogBoard(result);
            PrintResult(result);
        }

        private static void PrintResult(Board result)
        {
            if (result.GetLinesCountOfLength(4, Cell.Yellow) > 0)
                Console.WriteLine("Yellow player won!");
            else if (result.GetLinesCountOfLength(4, Cell.Red) > 0)
                Console.WriteLine("Red player won!");
            else
                Console.WriteLine("Draw!");
        }
    }
}