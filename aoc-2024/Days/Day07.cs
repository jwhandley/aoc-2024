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

    public override string Part1() => equations.AsParallel()
        .Where(e => SolveRecursive(e.Target, e.Numbers)).Sum(e => e.Target).ToString();

    public override string Part2() => equations.AsParallel()
        .Where(e => SolveRecursive(e.Target, e.Numbers, true)).Sum(e => e.Target).ToString();

    private static bool SolveRecursive(long target, long[] numbers, bool useConcat = false, int idx = 0)
    {
        if (idx == numbers.Length-1) return target == numbers[0];
        long currentNumber = numbers[numbers.Length - 1 - idx];
        if (CanDivide(target, currentNumber) && SolveRecursive(target / currentNumber, numbers, useConcat, idx+1)) return true;
        if (CanSubtract(target, currentNumber) && SolveRecursive(target - currentNumber, numbers, useConcat, idx+1)) return true;
        if (!useConcat || !CanStripSuffix(target, currentNumber, out long nextTarget)) return false;
        return SolveRecursive(nextTarget, numbers,  useConcat, idx+1);
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