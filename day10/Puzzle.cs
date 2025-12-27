internal record Machine(string Lights, int[][] Buttons, int[] Joltages);

internal class Puzzle(string rawInput) : AocPuzzle<Machine[], long>(rawInput)
{
    private const int NoCount = int.MaxValue;

    private static int GetTargetValue(string lights)
    {
        int target = 0;
        for (int i = 0; i < lights.Length; ++i)
        {
            if (lights[i] == '#')
                target |= 1 << i;
        }
        return target;
    }

    private static int GetButtonValue(int[] button)
    {
        int target = 0;
        for (int i = 0; i < button.Length; ++i)
        {
            target |= 1 << button[i];
        }
        return target;
    }

    private static int[][] GetCombinations(int[] values, int length)
    {
        if (length == 1)
            return [..values.Select(item => (int[])[item])];

        return GetCombinations(values, length - 1)
            .SelectMany(
                items => values.Where(item => item > items[^1]),
                (items, item) => (int[])[..items, item])
            .ToArray();
    }

    private static int GetLowestCount(Machine machine)
    {
        int lowestCount = NoCount;
        int targetValue = GetTargetValue(machine.Lights);
        int[] buttonValues = [..machine.Buttons.Select(GetButtonValue)];
        int[] values = [..Enumerable.Range(0, machine.Buttons.Length)];
        for (int i = 1; i <= machine.Buttons.Length; ++i)
        {
            var combinations = GetCombinations(values, i);
            foreach (var combination in combinations)
            {
                int count = 0;
                int value = 0;
                foreach (int buttonIndex in combination)
                {
                    if (++count > lowestCount)
                        break;

                    value ^= buttonValues[buttonIndex];
                    if (value == targetValue)
                    {
                        lowestCount = count;
                        break;
                    }
                }
            }
        }
        return lowestCount;
    }

    private static bool AreEqual(double a, double b) => Math.Abs(a - b) < 1e-5;

    private static double RoundIfInteger(double value)
    {
        double rounded = Math.Round(value);
        return AreEqual(value, rounded) ? rounded : value;
    }

    private static double[][] GetReducedMatrix(Machine machine, int[] combination)
    {
        var matrix = new double[machine.Joltages.Length][];
        for (int i = 0; i < matrix.Length; ++i)
        {
            matrix[i] = new double[machine.Buttons.Length + 1];
            matrix[i][^1] = machine.Joltages[i];
        }
        foreach (int buttonIndex in combination)
        {
            foreach (int joltageIndex in machine.Buttons[buttonIndex])
            {
                matrix[joltageIndex][buttonIndex] = 1;
            }
        }

        int m = matrix[0].Length - 1;
        int n = matrix.Length;
        for (int col = 0, row = 0; col < m && row < n; ++col)
        {
            int tmp = row;
            for (int i = row + 1; i < n; ++i)
            {
                if (Math.Abs(matrix[i][col]) > Math.Abs(matrix[tmp][col]))
                    tmp = i;
            }

            if (matrix[tmp][col] == 0)
                continue;

            (matrix[tmp], matrix[row]) = (matrix[row], matrix[tmp]);

            double divisor = matrix[row][col];
            if (divisor != 1)
            {
                for (int j = col; j <= m; ++j)
                {
                    matrix[row][j] = RoundIfInteger(matrix[row][j] / divisor);
                }
            }

            for (int i = 0; i < n; ++i)
            {
                if (i != row)
                {
                    double factor = matrix[i][col] / matrix[row][col];
                    for (int j = col; j <= m; ++j)
                    {
                        matrix[i][j] = RoundIfInteger(matrix[i][j] - factor * matrix[row][j]);
                    }
                }
            }

            ++row;
        }

        return matrix;
    }

    private static int GetButtonPresses(Machine machine, double[][] matrix)
    {
        var rows = matrix.Where(row => row.AsSpan().ContainsAnyExcept(0)).ToArray();
        if (rows.Any(row => !row.AsSpan()[..^1].ContainsAnyExcept(0)))
            return NoCount;

        if (rows.All(row => row.AsSpan()[..^1].Count(0) == row.Length - 2))
        {
            var presses = rows.Select(row => row[^1]).ToArray();
            return presses.Any(val => val < 1 || !double.IsInteger(val))
                ? NoCount
                : (int)presses.Sum();
        }

        HashSet<int> freeIndicesSet = [];
        for (int y = 0; y < matrix.Length; ++y)
        {
            int leadingIndex = matrix[y].IndexOf(1);
            for (int x = machine.Buttons.Length - 1; x > leadingIndex; --x)
            {
                if (matrix[y][x] != 0)
                    freeIndicesSet.Add(x);
            }
        }

        var freeIndices = freeIndicesSet.Order().ToArray();
        int lowestCount = NoCount;

        void CheckFreeIndices(int[] freePresses)
        {
            int curIndex = freePresses.Length;
            if (curIndex == freeIndices.Length)
            {
                int[] joltages = [..machine.Joltages];

                int count = freePresses.Sum();
                for (int i = 0; i < freePresses.Length; ++i)
                {
                    foreach (int joltageIndex in machine.Buttons[freeIndices[i]])
                    {
                        joltages[joltageIndex] -= freePresses[i];
                    }
                }

                foreach (var row in rows)
                {
                    double val = row[^1];
                    for (int i = 0; i < freePresses.Length; ++i)
                    {
                        val -= freePresses[i] * row[freeIndices[i]];
                    }

                    double rounded = RoundIfInteger(val);
                    if (rounded < 1 || !double.IsInteger(rounded))
                        return;

                    count += (int)rounded;
                    int buttonIndex = row.IndexOf(1);
                    foreach (int joltageIndex in machine.Buttons[buttonIndex])
                    {
                        joltages[joltageIndex] -= (int)rounded;
                    }
                }

                if (!joltages.AsSpan().ContainsAnyExcept(0))
                    lowestCount = Math.Min(count, lowestCount);
            }
            else
            {
                int max = machine.Buttons[freeIndices[curIndex]].Min(b => machine.Joltages[b]);
                for (int i = 1; i <= max; ++i)
                {
                    CheckFreeIndices([..freePresses, i]);
                }
            }
        }

        CheckFreeIndices([]);
        return lowestCount;
    }

    private static int GetLowestCount2(Machine machine)
    {
        int lowestCount = NoCount;
        for (int i = 1; i <= machine.Buttons.Length; ++i)
        {
            var combinations = GetCombinations([..Enumerable.Range(0, machine.Buttons.Length)], i);
            foreach (var combination in combinations)
            {
                var matrix = GetReducedMatrix(machine, combination);
                int count = GetButtonPresses(machine, matrix);
                lowestCount = Math.Min(count, lowestCount);
            }
        }
        return lowestCount;
    }

    private static Machine CreateMachine(string line)
    {
        var parts = line.Split(' ');
        var buttons = parts[1..^1].Select(part => part[1..^1].Split(',').Select(int.Parse).ToArray());
        var joltages = parts[^1][1..^1].Split(',').Select(int.Parse);
        return new(parts[0][1..^1], [..buttons], [..joltages]);
    }

    protected override Machine[] ParseInput(string rawInput) => [..rawInput.Split('\n').Select(CreateMachine)];

    protected override long RunPartOne() => _input.Sum(GetLowestCount);

    protected override long RunPartTwo() => _input.Sum(GetLowestCount2);
}
