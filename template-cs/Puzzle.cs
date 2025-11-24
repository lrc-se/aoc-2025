internal class Puzzle(string rawInput) : AocPuzzle<int[], int>(rawInput)
{
    protected override int[] ParseInput(string rawInput) => [..rawInput.Split('\n').Select(int.Parse)];

    protected override int RunPartOne() => _input.Sum();

    protected override int RunPartTwo() => _input.Aggregate((prev, cur) => prev * cur);
}
