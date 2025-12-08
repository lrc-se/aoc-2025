using System.Runtime.InteropServices;
using Coord = (int X, int Y, int Z);

internal record Connection(Coord First, Coord Second);

internal record Pair(Connection Connection, double Distance);

internal class Puzzle(string rawInput) : AocPuzzle<Coord[], long>(rawInput)
{
    private static Coord CreateCoord(string line)
    {
        var parts = line.Split(',');
        return new(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
    }

    private int GetCount() => _input.Length > 20 ? 1000 : 10;

    private List<Pair> GetPairs()
    {
        List<Pair> pairs = [];
        for (int i = 0; i < _input.Length - 1; ++i)
        {
            for (int j = i + 1; j < _input.Length; ++j)
            {
                double x = _input[i].X - _input[j].X;
                double y = _input[i].Y - _input[j].Y;
                double z = _input[i].Z - _input[j].Z;
                pairs.Add(new(new(_input[i], _input[j]), Math.Sqrt(x * x + y * y + z * z)));
            }
        }
        return pairs;
    }

    private static List<HashSet<Coord>> GetCircuits(Dictionary<Coord, List<Coord>> connections, bool onlyFirst = false)
    {
        List<HashSet<Coord>> circuits = [];
        HashSet<Coord> consumed = [];
        Queue<Coord> queue = [];
        foreach (var connection in onlyFirst ? connections.Take(1) : connections)
        {
            if (consumed.Contains(connection.Key))
                continue;

            HashSet<Coord> circuit = [connection.Key];
            consumed.Add(connection.Key);
            foreach (var conn in connection.Value)
            {
                queue.Enqueue(conn);
            }

            while (queue.Count > 0)
            {
                var coord = queue.Dequeue();
                if (consumed.Contains(coord))
                    continue;

                circuit.Add(coord);
                consumed.Add(coord);
                foreach (var conn in connections[coord])
                {
                    queue.Enqueue(conn);
                }
            }

            if (circuit.Count > 0)
                circuits.Add(circuit);
        }

        return circuits;
    }

    private static void AddConnection(Dictionary<Coord, List<Coord>> connections, Connection connection)
    {
        ref var conn1 = ref CollectionsMarshal.GetValueRefOrAddDefault(connections, connection.First, out bool exists);
        if (!exists)
            conn1 = [];

        ref var conn2 = ref CollectionsMarshal.GetValueRefOrAddDefault(connections, connection.Second, out exists);
        if (!exists)
            conn2 = [];

        conn1!.Add(connection.Second);
        conn2!.Add(connection.First);
    }

    protected override Coord[] ParseInput(string rawInput) => [..rawInput.Split('\n').Select(CreateCoord)];

    protected override long RunPartOne()
    {
        var pairs = GetPairs();
        Dictionary<Coord, List<Coord>> connections = [];
        foreach (var pair in pairs.OrderBy(p => p.Distance).Take(GetCount()))
        {
            AddConnection(connections, pair.Connection);
        }
        return GetCircuits(connections)
            .OrderByDescending(circuit => circuit.Count)
            .Take(3)
            .Aggregate(1L, (acc, circuit) => acc * circuit.Count);
    }

    protected override long RunPartTwo()
    {
        Pair[] pairs = [..GetPairs().OrderBy(pair => pair.Distance)];
        Dictionary<Coord, List<Coord>> connections = [];
        int count = GetCount();
        for (int i = 0; i < count; ++i)
        {
            AddConnection(connections, pairs[i].Connection);
        }
        for (int i = count; i < pairs.Length; ++i)
        {
            AddConnection(connections, pairs[i].Connection);
            var circuits = GetCircuits(connections, true);
            if (circuits[0].Count == _input.Length)
                return (long)pairs[i].Connection.First.X * pairs[i].Connection.Second.X;
        }
        return 0;
    }
}
