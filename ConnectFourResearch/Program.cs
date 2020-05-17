using System;
using ConnectFourResearch.ConnectFour;
using ConnectFourResearch.Logging;
using ConnectFourResearch.Solvers;

namespace ConnectFourResearch
{
    public class Program
    {
        public static void Main()
        {
            var logger = new ConsoleBoardLogger();
            var controller = new Controller(new ConsoleSolver(), new ConsoleSolver(), logger);
            var result = controller.Play();

            Console.Clear();
            logger.LogBoard(result);
            if (result.GetLinesCountOfLength(4, Cell.Yellow) > 0)
                Console.WriteLine("Yellow player won!");
            else if (result.GetLinesCountOfLength(4, Cell.Red) > 0)
                Console.WriteLine("Red player won!");
            else
                Console.WriteLine("Draw!");
        }
    }
}