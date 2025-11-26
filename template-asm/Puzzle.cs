using System.Runtime.InteropServices;

internal partial class Puzzle(string rawInput) : AocPuzzle<int[], int>(rawInput)
{
    [LibraryImport("bin/puzzle.dll")]
    private static partial int PartOne(int length, [In] int[] input);

    [LibraryImport("bin/puzzle.dll")]
    private static partial int PartTwo(int length, [In] int[] input);

    protected override int[] ParseInput(string rawInput) => [..rawInput.Split('\n').Select(int.Parse)];

    protected override int RunPartOne() => PartOne(_input.Length, _input);

    protected override int RunPartTwo() => PartTwo(_input.Length, _input);
}
