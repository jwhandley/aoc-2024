using System.Text.RegularExpressions;

namespace aoc_2024.Days;

public partial class Day03 : BaseDay
{
    public override string Part1() => MulRegex()
        .Matches(Input)
        .Sum(match => MulProduct(match.Value)).ToString();
    
    public override string Part2() => Part2Regex()
        .Matches(Input)
        .Aggregate((active: true, sum: 0L), (state, match) => match.Value switch
        {
            "do()" => (true, state.sum),
            "don't()" => (false, state.sum),
            _ => (state.active, state.active ? state.sum + MulProduct(match.Value) : state.sum)
        }).sum.ToString();

    private static int MulProduct(string match)
    {
        ReadOnlySpan<char> span = match.AsSpan(4, match.Length - 5);
        int commaIndex = span.IndexOf(',');
        int num1 = int.Parse(span[..commaIndex]);
        int num2 = int.Parse(span[(commaIndex + 1)..]);
        return num1 * num2;
    }

    [GeneratedRegex(@"mul\(\d+,\d+\)")]
    private static partial Regex MulRegex();


    [GeneratedRegex(@"do\(\)|don't\(\)|mul\(\d{1,3},\d{1,3}\)")]
    private static partial Regex Part2Regex();
}