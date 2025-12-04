using Coord = (int X, int Y);

internal class Puzzle(string rawInput) : AocPuzzle<HashSet<Coord>, int>(rawInput)
{
    private static readonly Coord[] _adjacent = [(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)];

    private bool CanBeAccessed(Coord coord)
    {
        int count = 0;
        foreach (var (x, y) in _adjacent)
        {
            if (_input.Contains((coord.X + x, coord.Y + y)) && ++count == 4)
                return false;
        }
        return true;
    }

    protected override HashSet<Coord> ParseInput(string rawInput)
    {
        HashSet<Coord> coords = [];
        var lines = rawInput.Split('\n');
        for (int y = 0; y < lines.Length; ++y)
        {
            for (int x = 0; x < lines[0].Length; ++x)
            {
                if (lines[y][x] == '@')
                    coords.Add((x, y));
            }
        }
        return coords;
    }

    protected override int RunPartOne() => _input.Count(CanBeAccessed);

    protected override int RunPartTwo()
    {
        int originalCount = _input.Count;
        while (_input.RemoveWhere(CanBeAccessed) > 0);
        return originalCount - _input.Count;
    }
}
