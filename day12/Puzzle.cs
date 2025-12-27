using System.Runtime.InteropServices;

internal unsafe struct Area
{
    public int Width;
    public int Height;
    public fixed int Presents[6];
}

internal partial class Puzzle(string rawInput) : AocPuzzle<Area[], int>(rawInput)
{
    [LibraryImport("bin/puzzle.dll")]
    private static partial int CountSpaciousAreas(int length, [In] Area[] input);

    private unsafe static Area CreateArea(string line)
    {
        var parts = line.Trim().Split(": ");
        var dimensions = parts[0].Split('x');
        var presents = parts[1].Split(' ');
        var area = new Area
        {
            Width = int.Parse(dimensions[0]),
            Height = int.Parse(dimensions[1])
        };
        for (int i = 0; i < 6; ++i)
        {
            area.Presents[i] = int.Parse(presents[i]);
        }
        return area;
    }

    protected override Area[] ParseInput(string rawInput) => [..rawInput.Split("\n\n")[^1].Split('\n').Select(CreateArea)];

    protected override int RunPartOne() => CountSpaciousAreas(_input.Length, _input);

    protected override int RunPartTwo() => 2025;
}
