using System.Collections.Generic;
using ConnectFourResearch.ConnectFour;

namespace ConnectFourResearch.Logging
{
    public interface IBoardLogger
    {
        void Log(Board board, IEnumerable<Move> moveVariants, Cell player);
        public void LogBoard(Board board);
    }
}
