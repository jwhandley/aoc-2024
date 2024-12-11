namespace aoc_2024.Days;

public class Day11 : BaseDay
{
    private readonly int[] startingNumbers;
    private readonly Dictionary<(long n, int depth), long> cache = [];

    public Day11()
    {
        startingNumbers = Input.Split(' ').Select(int.Parse).ToArray();
    }

    public override long Part1() => startingNumbers.Sum(n => DepthFirstSearch(n, 25));

    private static int CountDigits(long number)
    {
        int count = 0;

        while (number > 0)
        {
            number /= 10;
            count++;
        }

        return count;
    }

    private static long IntPow(long number, int power)
    {
        long result = 1;
        for (int i = 0; i < power; i++)
        {
            result *= number;
        }

        return result;
    }

    private long DepthFirstSearch(long number, int maxDepth, int depth = 0)
    {
        if (depth == maxDepth) return 1;
        if (cache.TryGetValue((number, depth), out long result)) return result;

        if (number == 0)
        {
            cache[(number, depth)] = DepthFirstSearch(1, maxDepth, depth + 1); 
            return cache[(number, depth)];
        }
        int digits = CountDigits(number);
        if (digits % 2 == 0)
        {
            long left = number / IntPow(10, digits / 2);
            long right = number % IntPow(10, digits / 2);
            cache[(number, depth)] = DepthFirstSearch(left, maxDepth, depth + 1) + DepthFirstSearch(right, maxDepth, depth + 1);
            return cache[(number, depth)];
        }

        cache[(number, depth)] = DepthFirstSearch(number * 2024, maxDepth, depth + 1);
        return cache[(number, depth)];
    }

    public override long Part2()
    {
        cache.Clear();
        return startingNumbers.Sum(n => DepthFirstSearch(n, 75));  
    } 
}