using Coord = (int X, int Y);

internal record Manifold(HashSet<Coord> Splitters, Coord Start, Coord Dimensions);

internal class Puzzle(string rawInput) : AocPuzzle<Manifold, long>(rawInput)
{
    private List<Coord> FindSplittersAbove(int x, int startY)
    {
        List<Coord> splitters = [];
        int left = x - 1;
        int right = x + 1;
        for (int y = startY - 2; y > 0; y -= 2)
        {
            if (_input.Splitters.Contains((x, y)))
                break;

            if (_input.Splitters.Contains((left, y)))
                splitters.Add((left, y));

            if (_input.Splitters.Contains((right, y)))
                splitters.Add((right, y));
        }
        return splitters;
    }

    private Dictionary<Coord, Coord[]> GetSplitterLinks()
    {
        Dictionary<Coord, Coord[]> links = [];
        foreach (var splitter in _input.Splitters)
        {
            links[splitter] = [..FindSplittersAbove(splitter.X, splitter.Y)];
        }
        links[(_input.Start.X, _input.Start.Y + 2)] = [_input.Start];
        return links;
    }

    private long CountPaths(Coord coord, Dictionary<Coord, Coord[]> links, Dictionary<Coord, long> cache)
    {
        if (coord == _input.Start)
            return 1;

        long curCount = 0;
        foreach (var link in links[coord])
        {
            if (!cache.TryGetValue(link, out long count))
                cache[link] = count = CountPaths(link, links, cache);

            curCount += count;
        }
        return curCount;
    }

    protected override Manifold ParseInput(string rawInput)
    {
        var lines = rawInput.Split('\n');
        Coord dimensions = (lines[0].Length, lines.Length);
        HashSet<Coord> splitters = [];
        for (int y = 2; y < dimensions.Y; y += 2)
        {
            for (int x = 0; x < dimensions.X; ++x)
            {
                if (lines[y][x] == '^')
                    splitters.Add((x, y));
            }
        }
        return new(splitters, (lines[0].IndexOf('S'), 0), dimensions);
    }

    protected override long RunPartOne() => GetSplitterLinks().Count(item => item.Value.Length > 0);

    protected override long RunPartTwo()
    {
        var links = GetSplitterLinks();
        List<Coord> lastSplitters = [];
        for (int x = 0; x < _input.Dimensions.X; ++x)
        {
            lastSplitters.AddRange(FindSplittersAbove(x, _input.Dimensions.Y));
        }
        Dictionary<Coord, long> cache = [];
        return lastSplitters.Sum(splitter => CountPaths(splitter, links, cache));
    }
}
