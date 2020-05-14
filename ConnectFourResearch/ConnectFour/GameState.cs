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

        /// <summary>
        /// Получить список возможных столбцов для выставления фишки
        /// </summary>
        /// <returns>номера незаполненных столбцов</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<int> GetPossibleMoves()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить количество линий определенной длины для определенного цвета
        /// </summary>
        /// <param name="length">длина линий (от 2 до 4)</param>
        /// <param name="player">цвет фишек</param>
        /// <returns>количество таких линий</returns>
        /// <exception cref="NotImplementedException"></exception>
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