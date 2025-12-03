internal class Puzzle(string rawInput) : AocPuzzle<string[], long>(rawInput)
{
    private static (char Max, int Index) FindMax(ReadOnlySpan<char> bank)
    {
        char max = bank[0];
        int index = 0;
        for (int i = 1; i < bank.Length; ++i)
        {
            if (bank[i] > max)
            {
                max = bank[i];
                index = i;
            }
        }
        return (max, index);
    }

    protected override string[] ParseInput(string rawInput) => rawInput.Split('\n');

    protected override long RunPartOne()
    {
        int result = 0;
        foreach (var bank in _input)
        {
            var span = bank.AsSpan();
            var (max1, index) = FindMax(span[..^1]);
            var (max2, _) = FindMax(span[(index + 1)..]);
            result += (max1 - '0') * 10 + max2 - '0';
        }
        return result;
    }

    protected override long RunPartTwo()
    {
        long result = 0;
        var digits = new char[12].AsSpan();
        foreach (var bank in _input)
        {
            var span = bank.AsSpan();
            int start = 0;
            for (int i = 0; i < 12; ++i)
            {
                var (max, index) = FindMax(span[start..^(11 - i)]);
                digits[i] = max;
                start += index + 1;
            }
            result += long.Parse(digits);
        }
        return result;
    }
}
