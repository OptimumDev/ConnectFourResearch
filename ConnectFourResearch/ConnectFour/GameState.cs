using System;
using System.Collections.Generic;

namespace ConnectFourResearch.ConnectFour
{
    public class GameState
    {
        public const int Width = 7;
        public const int Height = 6;

        private ulong redPositions;
        private ulong yellowPositions;

        public GameState() {}

        private GameState(ulong redPositions, ulong yellowPositions, int hash)
        {
        }

        public GameState Move(int column, CellState player)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetPossibleMoves()
        {
            throw new NotImplementedException();
        }

        public int GetLinesCountOfLength(int length, CellState player)
        {
            throw new NotImplementedException();
        }

        public bool IsFinished()
        {
            throw new NotImplementedException();
        }
    }
}