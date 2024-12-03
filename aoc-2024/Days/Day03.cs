using System.Text.RegularExpressions;

namespace aoc_2024.Days;

public partial class Day03 : BaseDay
{
    public override long Part1()
    {
        long result = 0;
        foreach (Match match in MulRegex().Matches(Input))
        {
            int[] nums = NumRegex().Matches(match.Value).Select(v => int.Parse(v.Value)).ToArray();
            result += nums[0] * nums[1];
        }

        return result;
    }

    public override long Part2()
    {
        List<(int start, int end)> invalidRanges = GetInvalidRanges();
        long result = 0;

        foreach (Match match in MulRegex().Matches(Input))
        {
            int matchIndex = match.Index;
            if (IsInInvalidRange(matchIndex, invalidRanges))
            {
                continue;
            }

            int[] nums = NumRegex().Matches(match.Value).Select(v => int.Parse(v.Value)).ToArray();
            result += nums[0] * nums[1];
        }

        return result;
    }

    private static bool IsInInvalidRange(int index, List<(int start, int end)> ranges)
    {
        int left = 0, right = ranges.Count - 1;
        while (left <= right)
        {
            int mid = (left + right) / 2;
            (int start, int end) = ranges[mid];
            if (index >= start && index < end)
                return true;
            if (index < start)
                right = mid - 1;
            else
                left = mid + 1;
        }

        return false;
    }


    [GeneratedRegex(@"mul\(\d+,\d+\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumRegex();

    [GeneratedRegex(@"don't\(\)")]
    private static partial Regex DoRegex();

    [GeneratedRegex(@"do\(\)")]
    private static partial Regex DontRegex();

    private List<(int start, int end)> GetInvalidRanges()
    {
        List<int> doNots = DoRegex().Matches(Input).Select(m => m.Index).ToList();
        List<int> dos = DontRegex().Matches(Input).Select(m => m.Index).ToList();

        List<(int start, int end)> invalidRanges =
            doNots.Select(start => (start, end: dos.FirstOrDefault(i => i > start))).ToList();

        return invalidRanges;
    }
}