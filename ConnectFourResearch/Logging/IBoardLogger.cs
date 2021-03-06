using System.Collections.Generic;
using ConnectFourResearch.ConnectFour;

namespace ConnectFourResearch.Logging
{
    public interface IBoardLogger
    {
        void Log(Board board, IEnumerable<Move> moveVariants, Cell player);
        void LogBoard(Board board);
        void PrintMoves(HashSet<int> moves);
    }
}
