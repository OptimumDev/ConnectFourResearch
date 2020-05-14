using ConnectFourResearch.Algorithms;

namespace ConnectFourResearch.ConnectFour
{
    public class GameMove : ISolution
    {
        public int Column { get; }
        public double Score { get; }

        public GameMove(int column, double score)
        {
            Column = column;
            Score = score;
        }
    }
}