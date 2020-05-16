using System;
using System.Collections.Generic;
using System.Linq;
using ConnectFourResearch.Extensions;

namespace ConnectFourResearch.ConnectFour
{
    public class Board
    {
        public const int Width = 7;
        public const int Height = 6;
        private const int Height1 = Height + 1;
        private const int Size = Width * Height1;
        private const ulong All = (1UL << Size) - 1;
        private const ulong Column = (1UL << Height1) - 1;
        private const ulong Bottom = All / Column;
        private const ulong Top = Bottom << (Height - 1);

        private static readonly byte[] DirectionOffsets = {1, 6, 7, 8};

        private readonly ulong _redPositions;
        private readonly ulong _yellowPositions;
        private readonly byte[] _heights;

        public Board() : this(0UL, 0UL, CreateHeights())
        {
        }

        private Board(ulong redPositions, ulong yellowPositions, byte[] heights, int hash = 0)
        {
            _redPositions = redPositions;
            _yellowPositions = yellowPositions;
            _heights = heights;
        }

        public Board Move(int column, Cell player)
        {
            var heights = new byte[_heights.Length];
            Array.Copy(_heights, heights, _heights.Length);
            var position = 1UL << heights[column]++;
            return player switch
            {
                Cell.Red => new Board(_redPositions ^ position, _yellowPositions, heights),
                Cell.Yellow => new Board(_redPositions, _yellowPositions ^ position, heights),
                _ => throw new ArgumentException($"A player can not be {player}")
            };
        }

        /// <summary>
        /// Получить список возможных столбцов для выставления фишки
        /// </summary>
        /// <returns>Номера незаполненных столбцов</returns>
        public IEnumerable<int> GetPossibleMoves()
        {
            for (var column = 0; column < Width; column++)
            {
                if (!IsColumnFull(column)) yield return column;
            }
        }

        /// <summary>
        /// Получить количество линий определенной длины для определенного цвета
        /// </summary>
        /// <param name="length">Длина линий (от 2 до 4)</param>
        /// <param name="player">Цвет фишек</param>
        /// <returns>Количество таких линий</returns>
        public int GetLinesCountOfLength(int length, Cell player)
        {
            var positions = GetPlayerPositions(player);
            var count = 0;
            foreach (var offset in DirectionOffsets)
            {
                var bitBoard = positions;
                for (var l = 1; l < length; l++)
                    bitBoard &= positions >> l * offset;
                count += bitBoard.BitCount();
            }

            return count;
        }

        public bool IsFinished()
        {
            for (var column = 0; column < Width; column++)
            {
                if (!IsColumnFull(column))
                    return GetLinesCountOfLength(4, Cell.Red) > 0 ||
                           GetLinesCountOfLength(4, Cell.Yellow) > 0;
            }

            return true;
        }

        public Cell GetCell(int x, int y)
        {
            if (IsSet(_redPositions, x, y)) return Cell.Red;
            if (IsSet(_yellowPositions, x, y)) return Cell.Yellow;
            return Cell.Empty;
        }

        private bool IsColumnFull(int column) => (Top & (1UL << _heights[column])) == 1;

        private ulong GetPlayerPositions(Cell player) => player switch
        {
            Cell.Red => _redPositions,
            Cell.Yellow => _yellowPositions,
            _ => throw new ArgumentException($"A player can not be {player}")
        };

        private static byte[] CreateHeights() => Enumerable.Range(0, Width)
            .Select(i => (byte) (i * Height1))
            .ToArray();

        private static bool IsSet(ulong positions, int x, int y) => positions >> (y + x * Height1) == 1;
    }
}