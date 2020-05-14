using ConnectFourResearch.Algorithms;

namespace ConnectFourResearch.ConnectFour
{
    public class Move : ISolution
    {
        public int Column { get; }
        public double Score { get; }

        public Move(int column, double score)
        {
            Column = column;
            Score = score;
        }
    }
}