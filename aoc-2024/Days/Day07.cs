namespace aoc_2024.Days;

public class Day07 : BaseDay
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

    public override long Part1() => equations.AsParallel()
        .Where(e => SolveRecursive(e.Target, e.Numbers, e.Numbers.Length - 1)).Sum(e => e.Target);

    public override long Part2() => equations.AsParallel()
        .Where(e => SolveRecursive(e.Target, e.Numbers, e.Numbers.Length - 1, true)).Sum(e => e.Target);

    private static bool SolveRecursive(long target, long[] numbers, int idx, bool part2 = false)
    {
        if (idx == 0) return target == numbers[0];

        long currentNumber = numbers[idx];
        if (CanDivide(target, currentNumber) && SolveRecursive(target / currentNumber, numbers, idx - 1, part2)) return true;
        if (CanSubtract(target, currentNumber) && SolveRecursive(target - currentNumber, numbers, idx - 1, part2)) return true;
        if (!part2 || !CanStripSuffix(target, currentNumber, out long nextTarget)) return false;
        return SolveRecursive(nextTarget, numbers, idx - 1, part2);
    }


    private static bool CanDivide(long target, long number) => target % number == 0;
    private static bool CanSubtract(long target, long number) => target > number;

    private static bool CanStripSuffix(long target, long number, out long nextTarget)
    {
        string targetStr = target.ToString();
        string numberStr = number.ToString();

        if (targetStr.Length > numberStr.Length && targetStr.EndsWith(numberStr))
        {
            nextTarget = long.Parse(targetStr[..^numberStr.Length]);
            return true;
        }

        nextTarget = 0;
        return false;
    }
}

public record struct Equation(long Target, long[] Numbers);