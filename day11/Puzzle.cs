using Devices = System.Collections.Generic.Dictionary<string, string[]>;

internal class Puzzle(string rawInput) : AocPuzzle<Devices, long>(rawInput)
{
    private long CountPaths(string from, string to)
    {
        long InnerCount(string current, Dictionary<string, long> cache)
        {
            if (current == to)
                return 1;

            if (!_input.TryGetValue(current, out var outputs))
                return 0;

            long curCount = 0;
            foreach (string output in outputs)
            {
                if (!cache.TryGetValue(output, out long count))
                    cache[output] = count = InnerCount(output, cache);

                curCount += count;
            }
            return curCount;
        }

        return InnerCount(from, []);
    }

    protected override Devices ParseInput(string rawInput)
    {
        Devices devices = [];
        foreach (string line in rawInput.Split('\n'))
        {
            var parts = line.Split(": ");
            devices[parts[0]] = parts[1].Split(' ');
        }
        return devices;
    }

    protected override long RunPartOne() => CountPaths("you", "out");

    protected override long RunPartTwo()
    {
        long count1 = CountPaths("svr", "dac") * CountPaths("dac", "fft") * CountPaths("fft", "out");
        long count2 = CountPaths("svr", "fft") * CountPaths("fft", "dac") * CountPaths("dac", "out");
        return count1 + count2;
    }
}
