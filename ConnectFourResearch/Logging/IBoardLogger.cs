using System.Collections;
using System.Collections.Generic;
using ConnectFourResearch.ConnectFour;

namespace ConnectFourResearch.Algorithms
{
    public interface IBoardLogger
    {
        void Log(Board board, IEnumerable<Move> moveVariants, Cell player);
    }
}
