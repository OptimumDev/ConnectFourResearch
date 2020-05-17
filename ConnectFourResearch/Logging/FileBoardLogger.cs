using System.IO;

namespace ConnectFourResearch.Logging
{
    public class FileBoardLogger : UniversalBoardLogger
    {
        public FileBoardLogger(string filePath) : base(
            s => File.AppendAllText(filePath, s),
            _ => File.AppendAllText(filePath, "Y"),
            _ => File.AppendAllText(filePath, "R")
        )
        {
            File.Create(filePath);
        }
    }
}
