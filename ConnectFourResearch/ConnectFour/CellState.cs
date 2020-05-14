using System;

namespace ConnectFourResearch.ConnectFour
{
    public enum CellState
    {
        Red,
        Yellow,
        Empty
    }

    public static class CellStateExtensions
    {
        public static CellState GetOpponent(this CellState player) => player switch
        {
            CellState.Red => CellState.Yellow,
            CellState.Yellow => CellState.Red,
            _ => throw new ArgumentException($"{CellState.Empty} can't have opponent")
        };
    }
}