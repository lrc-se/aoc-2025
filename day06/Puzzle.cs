using System.Text.RegularExpressions;

internal record Input(string[] Rows, char[] Operations);

internal class Puzzle(string rawInput) : AocPuzzle<Input, long>(rawInput)
{
    private static readonly Regex _splitRe = new(@"\s+", RegexOptions.Compiled);

    protected override Input ParseInput(string rawInput)
    {
        var lines = rawInput.Split('\n');
        var operations = _splitRe.Split(lines[^1].Trim()).Select(op => op[0]);
        return new(lines[..^1], [..operations]);
    }

    protected override long RunPartOne()
    {
        long result = 0;
        var numberRows = _input.Rows
            .Select(row => _splitRe.Split(row.Trim()).Select(int.Parse).ToArray())
            .ToArray();
        for (int i = 0; i < _input.Operations.Length; ++i)
        {
            result += _input.Operations[i] == '+'
                ? numberRows.Sum(row => row[i])
                : numberRows.Aggregate(1L, (acc, row) => acc * row[i]);
        }
        return result;
    }

    protected override long RunPartTwo()
    {
        long result = 0;
        var numbers = new List<int>(4);
        var digits = new char[_input.Rows.Length].AsSpan();
        int operationIndex = _input.Operations.Length - 1;
        for (int x = _input.Rows[0].Length - 1; x >= 0; --x)
        {
            for (int y = 0; y < _input.Rows.Length; ++y)
            {
                digits[y] = _input.Rows[y][x];
            }

            bool isNumber = digits.ContainsAnyExcept(' ');
            if (isNumber)
                numbers.Add(int.Parse(digits));

            if (!isNumber || x == 0)
            {
                result += _input.Operations[operationIndex--] == '+'
                    ? numbers.Sum()
                    : numbers.Aggregate(1L, (acc, num) => acc * num);

                numbers.Clear();
            }
        }
        return result;
    }
}
