using System.Runtime.InteropServices;
using Coord = (int X, int Y);

internal class Puzzle(string rawInput) : AocPuzzle<Coord[], long>(rawInput)
{
    private static Coord CreateCoord(string line)
    {
        var parts = line.Split(',');
        return new(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    protected override Coord[] ParseInput(string rawInput) => [..rawInput.Split('\n').Select(CreateCoord)];

    protected override long RunPartOne()
    {
        long result = 0;
        for (int i = 0; i < _input.Length - 2; ++i)
        {
            for (int j = i + 2; j < _input.Length; ++j)
            {
                long width = Math.Abs(_input[i].X - _input[j].X) + 1;
                long height = Math.Abs(_input[i].Y - _input[j].Y) + 1;
                long area = width * height;
                if (area > result)
                    result = area;
            }
        }
        return result;
    }

    protected override long RunPartTwo()
    {
        long result = 0;
        List<Coord> tiles = [];
        for (int i = 1; i <= _input.Length; ++i)
        {
            var coord1 = _input[i - 1];
            var coord2 = i < _input.Length ? _input[i] : _input[0];
            Coord delta = coord1.X == coord2.X
                ? (0, Math.Sign(coord2.Y - coord1.Y))
                : (Math.Sign(coord2.X - coord1.X), 0);
            var coord = coord1;
            while (coord != coord2)
            {
                tiles.Add(coord);
                coord.X += delta.X;
                coord.Y += delta.Y;
            }
        }

        _input.Sort();
        var tilesSpan = CollectionsMarshal.AsSpan(tiles);
        List<(Coord NW, Coord SE)> impossibleAreas = [];
        for (int i = 0; i < _input.Length - 1; ++i)
        {
            for (int j = i + 1; j < _input.Length; ++j)
            {
                if (_input[i].X == _input[j].X || _input[i].Y == _input[j].Y)
                    continue;

                (int startX, int endX) = _input[i].X < _input[j].X
                    ? (_input[i].X, _input[j].X)
                    : (_input[j].X, _input[i].X);
                (int startY, int endY) = _input[i].Y < _input[j].Y
                    ? (_input[i].Y, _input[j].Y)
                    : (_input[j].Y, _input[i].Y);

                bool possible = true;
                foreach (var (nw, se) in impossibleAreas)
                {
                    if (startX <= nw.X && endX >= se.X && startY <= nw.Y && endY >= se.Y)
                    {
                        possible = false;
                        break;
                    }
                }
                if (!possible)
                    continue;

                foreach (var (x, y) in tilesSpan)
                {
                    if (x > startX && x < endX && y > startY && y < endY)
                    {
                        possible = false;
                        impossibleAreas.Add(((startX, startY), (endX, endY)));
                        break;
                    }
                }
                if (possible)
                {
                    long width = endX - startX + 1;
                    long height = endY - startY + 1;
                    long area = width * height;
                    if (area > result)
                        result = area;
                }
            }
        }

        return result;
    }
}
