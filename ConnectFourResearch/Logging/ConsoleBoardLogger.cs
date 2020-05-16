using System;

namespace ConnectFourResearch.Logging
{
    public class ConsoleBoardLogger : UniversalBoardLogger
    {
        public ConsoleBoardLogger() : base(
            Console.Write,
            GetColorLogAction(ConsoleColor.Yellow),
            GetColorLogAction(ConsoleColor.Red)
        ) { }

        private static Action<string> GetColorLogAction(ConsoleColor color) => str =>
        {
            var previousColor = Console.ForegroundColor;

            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ForegroundColor = previousColor;
        };
    }
}
