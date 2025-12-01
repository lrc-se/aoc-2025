internal class Puzzle(string rawInput) : AocPuzzle<int[], int>(rawInput)
{
    private static int Mod(int a, int b) => ((a % b) + b) % b;

    protected override int[] ParseInput(string rawInput)
        => rawInput
            .Split('\n')
            .Select(line => line[0] == 'L' ? -int.Parse(line[1..]) : int.Parse(line[1..]))
            .ToArray();

    protected override int RunPartOne()
    {
        int result = 0;
        int position = 50;
        foreach (int rotation in _input)
        {
            position = Mod(position + rotation, 100);
            if (position == 0)
                ++result;
        }
        return result;
    }

    protected override int RunPartTwo()
    {
        int result = 0;
        int position = 50;
        foreach (int rotation in _input)
        {
            int nextPosition = position + rotation;
            result += Math.Abs(nextPosition) / 100;
            if (position > 0 && nextPosition <= 0)
                ++result;

            position = Mod(nextPosition, 100);
        }
        return result;
    }
}
