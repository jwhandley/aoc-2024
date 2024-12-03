using System.Text.RegularExpressions;

namespace aoc_2024.Days;

public partial class Day03 : BaseDay
{
    public override long Part1() => MulRegex()
        .Matches(Input)
        .Select(v =>
            NumRegex()
                .Matches(v.Value)
                .Take(2)
                .Select(m => int.Parse(m.Value))
                .Aggregate((a, b) => a * b))
        .Sum();


    public override long Part2() => Part2Regex()
            .Matches(Input)
            .Aggregate((active: true, sum: 0L), (state, match) =>
            {
                (bool active, long sum) = state;
                return match.Value switch
                {
                    "do()" => (true, sum),
                    "don't()" => (false, sum),
                    _ when active => (active, sum + NumRegex()
                        .Matches(match.Value)
                        .Take(2)
                        .Select(m => int.Parse(m.Value))
                        .Aggregate((a, b) => a * b)),
                    _ => state
                };
            }).sum;
    


    [GeneratedRegex(@"mul\(\d+,\d+\)")]
    private static partial Regex MulRegex();

    [GeneratedRegex(@"\d+")]
    private static partial Regex NumRegex();

    [GeneratedRegex(@"do\(\)|don't\(\)|mul\(\d{1,3},\d{1,3}\)")]
    private static partial Regex Part2Regex();
}