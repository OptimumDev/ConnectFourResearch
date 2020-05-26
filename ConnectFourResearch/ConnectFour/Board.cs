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
        private const ulong Top = Bottom << Height;

        private static readonly byte[] DirectionOffsets = {1, 6, 7, 8};
        private static readonly int[,] ZobristHashTable = CreateZobristHashTable();

        private readonly ulong _redPositions;
        private readonly ulong _yellowPositions;
        private readonly byte[] _heights;
        private readonly int _hash;

        public Board() : this(0UL, 0UL, CreateHeights())
        {
        }

        private Board(ulong redPositions, ulong yellowPositions, byte[] heights, int hash = 0)
        {
            _redPositions = redPositions;
            _yellowPositions = yellowPositions;
            _heights = heights;
            _hash = hash;
        }

        public override int GetHashCode() => _hash;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals((Board) obj);
        }

        private bool Equals(Board other)
        {
            return _redPositions == other._redPositions &&
                   _yellowPositions == other._yellowPositions;
        }

        public Board Move(int column, Cell player)
        {
            var heights = new byte[_heights.Length];
            Array.Copy(_heights, heights, _heights.Length);
            var position = 1UL << heights[column]++;
            var hash = CalculateNextStateHash(column, player);
            return player switch
            {
                Cell.Red => new Board(_redPositions ^ position, _yellowPositions, heights, hash),
                Cell.Yellow => new Board(_redPositions, _yellowPositions ^ position, heights, hash),
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

        private int CalculateNextStateHash(int column, Cell cell)
        {
            return _hash ^ ZobristHashTable[_heights[column], (int) cell];
        }

        private bool IsColumnFull(int column) => IsTopRow(_heights[column]);

        private ulong GetPlayerPositions(Cell player) => player switch
        {
            Cell.Red => _redPositions,
            Cell.Yellow => _yellowPositions,
            _ => throw new ArgumentException($"A player can not be {player}")
        };

        private static int[,] CreateZobristHashTable()
        {
            var random = new Random();
            var cellTypes = Enum.GetValues(typeof(Cell));
            var table = new int[Size, cellTypes.Length];
            foreach (var j in cellTypes.Cast<int>())
            {
                for (var i = 0; i < Size; i++)
                {
                    if (IsTopRow(i))
                        continue;
                    table[i, j] = random.Next();
                }
            }

            return table;
        }

        private static bool IsTopRow(int index) => (Top & (1UL << index)) != 0;

        private static byte[] CreateHeights() => Enumerable.Range(0, Width)
            .Select(i => (byte) (i * Height1))
            .ToArray();

        private static bool IsSet(ulong positions, int x, int y) => ((positions >> (y + x * Height1)) & 1) == 1;
    }
}