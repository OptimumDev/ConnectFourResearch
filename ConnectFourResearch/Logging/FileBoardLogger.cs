using System;
using System.IO;

namespace ConnectFourResearch.Algorithms
{
    public class FileBoardLogger : UniversalBoardLogger
    {
        public FileBoardLogger(string filePath) : base(
            GetLogAction(filePath),
            GetLogAction(filePath),
            GetLogAction(filePath)
        )
        {
            File.Create(filePath);
        }

        private static Action<string> GetLogAction(string filePath) => str =>
            File.AppendAllText(filePath, str);
    }
}
