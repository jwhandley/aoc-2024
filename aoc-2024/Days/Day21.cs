using System.Text.RegularExpressions;

namespace aoc_2024.Days;

public partial class Day21 : BaseDay
{
    private readonly string[] targets;
    private readonly Dictionary<char, (int r, int c)> numpad;
    private readonly Dictionary<char, (int r, int c)> directionalKeyPad;
    private readonly Dictionary<(char, char, int, bool), long> cache;

    public Day21()
    {
        cache = new Dictionary<(char, char, int, bool), long>();
        targets = Input.Split(Environment.NewLine);
        numpad = [];
        numpad['A'] = (0, 2);
        numpad['0'] = (0, 1);

        numpad['1'] = (1, 0);
        numpad['2'] = (1, 1);
        numpad['3'] = (1, 2);

        numpad['4'] = (2, 0);
        numpad['5'] = (2, 1);
        numpad['6'] = (2, 2);

        numpad['7'] = (3, 0);
        numpad['8'] = (3, 1);
        numpad['9'] = (3, 2);

        directionalKeyPad = [];
        directionalKeyPad['A'] = (1, 2);
        directionalKeyPad['^'] = (1, 1);
        directionalKeyPad['<'] = (0, 0);
        directionalKeyPad['v'] = (0, 1);
        directionalKeyPad['>'] = (0, 2);
    }

    private IEnumerable<string> WaysToMake(char start, char target, bool useNumpad)
    {
        var targetPos = useNumpad ? numpad[target] : directionalKeyPad[target];
        var startPos = useNumpad ? numpad[start] : directionalKeyPad[start];
        (int r, int c) illegal = useNumpad ? (0, 0) : (1, 0);

        Stack<(int r, int c, string current)> stack = new();
        stack.Push((startPos.r, startPos.c, ""));

        while (stack.Count > 0)
        {
            (int r, int c, string current) = stack.Pop();

            if ((r, c) == targetPos) yield return current + 'A';

            if (r > targetPos.r && (r - 1, c) != illegal)
            {
                stack.Push((r - 1, c, current + 'v'));
            }

            if (r < targetPos.r && (r + 1, c) != illegal)
            {
                stack.Push((r + 1, c, current + '^'));
            }

            if (c > targetPos.c && (r, c - 1) != illegal)
            {
                stack.Push((r, c - 1, current + '<'));
            }

            if (c < targetPos.c && (r, c + 1) != illegal)
            {
                stack.Push((r, c + 1, current + '>'));
            }
        }
    }

    private long Solve(string target, int depth)
    {
        long result = 0;
        char last = 'A';
        foreach (char t in target)
        {
            result += MinCost(last, t, depth, true);
            last = t;
        }

        return result;

        long MinCost(char start, char t, int d, bool useNumpad)
        {
            if (cache.TryGetValue((start, t, d, useNumpad), out long cost)) return cost;
            if (d == 0) return WaysToMake(start, t, useNumpad).Select(s => s.Length).Min();

            long minCost = long.MaxValue;
            foreach (string way in WaysToMake(start, t, useNumpad))
            {
                long tmp = 0;
                char prev = 'A';
                foreach (char c in way)
                {
                    long best = MinCost(prev, c, d - 1, false);
                    prev = c;
                    tmp += best;
                }

                minCost = Math.Min(minCost, tmp);
            }

            cache[(start, t, d, useNumpad)] = minCost;
            return minCost;
        }
    }

    public override string Part1() => targets.Sum(t => Solve(t, 2) * long.Parse(NumRegex().Match(t).Value)).ToString();


    public override string Part2() => targets.Sum(t => Solve(t, 25) * long.Parse(NumRegex().Match(t).Value)).ToString();

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumRegex();
}