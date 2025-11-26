using System.Runtime.InteropServices;
using CupLinks = System.Collections.Generic.Dictionary<int, int>;

internal class Puzzle(string rawInput) : AocPuzzle<int[], string>(rawInput)
{
    private static void PopulateCupList(Span<int> cups, CupLinks links, int startCup)
    {
        int curCup = startCup;
        for (int i = 0; i < cups.Length; ++i)
        {
            cups[i] = curCup;
            curCup = links[curCup];
        }
    }

    private CupLinks CreateCupLinks(int totalCount)
    {
        Span<int> cups;
        if (_input.Length < totalCount)
        {
            cups = new int[totalCount].AsSpan();
            _input.AsSpan().CopyTo(cups[.._input.Length]);
            for (int i = _input.Length; i < totalCount; ++i)
            {
                cups[i] = i + 1;
            }
        }
        else
            cups = _input;

        CupLinks links = new(totalCount);
        for (int i = 1; i < cups.Length; ++i)
        {
            links[cups[i - 1]] = cups[i];
        }
        links[cups[^1]] = cups[0];

        return links;
    }

    private void MoveCups(CupLinks links, int moves)
    {
        int curCup = _input[0];
        Span<int> pickedUp = new int[3];
        for (int i = 0; i < moves; ++i)
        {
            ref int curLink = ref CollectionsMarshal.GetValueRefOrNullRef(links, curCup);
            PopulateCupList(pickedUp, links, curLink);
            int destCup = curCup > 1 ? curCup - 1 : links.Count;
            while (pickedUp.Contains(destCup))
            {
                destCup = destCup > 1 ? destCup - 1 : links.Count;
            }

            ref int lastLink = ref CollectionsMarshal.GetValueRefOrNullRef(links, pickedUp[2]);
            ref int destLink = ref CollectionsMarshal.GetValueRefOrNullRef(links, destCup);
            curCup = curLink = lastLink;
            lastLink = destLink;
            destLink = pickedUp[0];
        }
    }

    protected override int[] ParseInput(string rawInput) => [..rawInput.Select(chr => chr - '0')];

    protected override string RunPartOne()
    {
        var links = CreateCupLinks(_input.Length);
        MoveCups(links, 100);
        var cups = new int[8];
        PopulateCupList(cups, links, links[1]);
        return string.Join("", cups);
    }

    protected override string RunPartTwo()
    {
        var links = CreateCupLinks(1_000_000);
        MoveCups(links, 10_000_000);
        var cups = new int[3];
        PopulateCupList(cups, links, 1);
        return ((long)cups[1] * cups[2]).ToString();
    }
}
