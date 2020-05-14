using System;

namespace ConnectFourResearch.ConnectFour
{
    public enum Cell
    {
        Red,
        Yellow,
        Empty
    }

    public static class CellStateExtensions
    {
        public static Cell GetOpponent(this Cell player) => player switch
        {
            Cell.Red => Cell.Yellow,
            Cell.Yellow => Cell.Red,
            _ => throw new ArgumentException($"{Cell.Empty} can't have opponent")
        };
    }
}