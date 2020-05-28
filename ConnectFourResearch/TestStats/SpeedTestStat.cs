namespace ConnectFourResearch.TestStats
{
    public class TestStat
    {
        public readonly int GamesCount;
        public readonly int WinsCount;
        public readonly int Score;
        public readonly string SolverName;

        public TestStat(int gamesCount, int winsCount, string solverName)
        {
            GamesCount = gamesCount;
            WinsCount = winsCount;
            Score = GetScore(winsCount, gamesCount);
            SolverName = solverName;
        }

        public override string ToString() => $"| {SolverName} | {GamesCount} | {WinsCount} | {Score} |";

        private static int GetScore(int winsCount, int gamesCount) => winsCount * 100 + gamesCount;
    }
}