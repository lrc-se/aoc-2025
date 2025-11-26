using System.Runtime.InteropServices;

internal partial class Puzzle(string rawInput) : AocPuzzle<int[][], int>(rawInput)
{
    [LibraryImport("bin/puzzle.dll")]
    private static partial int IsSafe(int length, [In] int[] input);

    [LibraryImport("bin/puzzle.dll")]
    private static partial int IsSafeDampened(int length, [In] int[] input);

    protected override int[][] ParseInput(string rawInput)
        => rawInput
            .Split('\n')
            .Select(line => line.Split(' ').Select(int.Parse).ToArray())
            .ToArray();

    protected override int RunPartOne() => _input.Sum(report => IsSafe(report.Length, report));

    protected override int RunPartTwo() => _input.Sum(report => IsSafeDampened(report.Length, report));
}
