internal abstract class AocPuzzle<TInput, TResult>
{
    protected AocPuzzle(string rawInput) => _input = ParseInput(rawInput);

    public string Run(string part) => part switch
    {
        "1" or "part1" => $"Part one result: {RunPartOne()}",
        "2" or "part2" => $"Part two result: {RunPartTwo()}",
        var unknown => $"Unknown part: '{unknown}'"
    };

    protected abstract TInput ParseInput(string rawInput);
    protected abstract TResult RunPartOne();
    protected abstract TResult RunPartTwo();

    protected TInput _input;
}
