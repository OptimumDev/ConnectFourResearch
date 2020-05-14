using System;
using System.Collections.Generic;

namespace ConnectFourResearch.ConnectFour
{
    public class Board
    {
        public const int Width = 7;
        public const int Height = 6;

        private ulong redPositions;
        private ulong yellowPositions;

        public Board() {}

        private Board(ulong redPositions, ulong yellowPositions, int hash)
        {
        }

        public Board Move(int column, Cell player)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить список возможных столбцов для выставления фишки
        /// </summary>
        /// <returns>Номера незаполненных столбцов</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<int> GetPossibleMoves()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Получить количество линий определенной длины для определенного цвета
        /// </summary>
        /// <param name="length">Длина линий (от 2 до 4)</param>
        /// <param name="player">Цвет фишек</param>
        /// <returns>Количество таких линий</returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetLinesCountOfLength(int length, Cell player)
        {
            throw new NotImplementedException();
        }

        public bool IsFinished()
        {
            throw new NotImplementedException();
        }

        public Cell GetCell(int x, int y)
        {
            throw new NotImplementedException();
        }
    }
}