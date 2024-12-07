namespace aoc_2024.Days;

public class Day07 :BaseDay
{
    private readonly Equation[] equations;

    public Day07()
    {
        equations = Input.Split(Environment.NewLine).Select(l =>
        {
            string[] parts = l.Split(": ");
            long target = long.Parse(parts[0]);
            long[] numbers = parts[1].Split(" ").Select(long.Parse).ToArray();
            return new Equation(target, numbers);
        }).ToArray();
    }

    public override long Part1() => equations.AsParallel().Where(e => SolveRecursive(e.Target, e.Numbers, e.Numbers.Length-1)).Sum(e => e.Target);
    
    public override long Part2() => equations.AsParallel().Where(e => SolveRecursive(e.Target, e.Numbers, e.Numbers.Length-1, true)).Sum(e => e.Target);

    private static bool SolveRecursive(long target, long[] numbers, int idx, bool part2 = false)
    {
        if (idx == 0) return target == numbers[idx];
        if (target % numbers[idx] == 0 && SolveRecursive(target / numbers[idx], numbers, idx - 1, part2)) return true;
        if (target > numbers[idx] && SolveRecursive(target - numbers[idx], numbers, idx - 1, part2)) return true;
        if (!part2) return false;
        string targetStr = target.ToString();
        string lastStr = numbers[idx].ToString();
        if (targetStr.Length <= lastStr.Length || !targetStr.EndsWith(lastStr)) return false;
        string nextTarget = targetStr[..^lastStr.Length];
        return SolveRecursive(long.Parse(nextTarget), numbers, idx - 1, part2);
    }
}

public record struct Equation(long Target, long[] Numbers);