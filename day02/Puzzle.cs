using Range = (long First, long Last);

internal class Puzzle(string rawInput) : AocPuzzle<Range[], long>(rawInput)
{
    private static Range CreateRange(string line)
    {
        var parts = line.Split('-');
        return new(long.Parse(parts[0]), long.Parse(parts[1]));
    }

    protected override Range[] ParseInput(string rawInput) => [..rawInput.Split(',').Select(CreateRange)];

    protected override long RunPartOne()
    {
        long result = 0;
        foreach (var (first, last) in _input)
        {
            (string firstString, string lastString) = (first.ToString(), last.ToString());
            if ((firstString.Length & 1) + (lastString.Length & 1) > 1)
                continue;

            for (long i = first; i <= last; ++i)
            {
                var curSpan = i.ToString().AsSpan();
                if ((curSpan.Length & 1) > 0)
                    continue;

                int midpoint = curSpan.Length / 2;
                if (curSpan[..midpoint].Equals(curSpan[midpoint..], StringComparison.Ordinal))
                    result += i;
            }
        }
        return result;
    }

    protected override long RunPartTwo()
    {
        long result = 0;
        foreach (var (first, last) in _input)
        {
            for (long i = first; i <= last; ++i)
            {
                var curSpan = i.ToString().AsSpan();
                int midpoint = curSpan.Length / 2;
                for (int j = 1; j <= midpoint; ++j)
                {
                    if (curSpan.Length % j > 0)
                        continue;

                    var pattern = curSpan[..j];
                    bool match = true;
                    for (int k = j; k < curSpan.Length; k += j)
                    {
                        if (!curSpan[k..(k + j)].Equals(pattern, StringComparison.Ordinal))
                        {
                            match = false;
                            break;
                        }
                    }

                    if (match)
                    {
                        result += i;
                        break;
                    }
                }
            }
        }
        return result;
    }
}
