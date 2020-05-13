namespace ConnectFourResearch.Algorithms
{
    public interface IMutation<out TResult>
    {
        double Score { get; }
        TResult GetResult();
    }
}