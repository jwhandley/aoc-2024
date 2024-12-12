namespace aoc_2024.Days;

public class Day11 : BaseDay
{
    private readonly int[] startingNumbers;
    private readonly Dictionary<(long n, int remaining), long> cache = [];

    public Day11()
    {
        startingNumbers = Input.Split(' ').Select(int.Parse).ToArray();
    }

    public override long Part1() => startingNumbers.Sum(n => DepthFirstSearch(n, 25));

    private static int CountDigits(long number) => number <= 9 ? 1 : (int)Math.Floor(Math.Log10(number)) + 1;
    

    private static (long left, long right) SplitNumber(long number, int digits)
    {
        long power = (long)Math.Pow(10, digits);
        return (number / power, number % power);
    }

    private long DepthFirstSearch(long number, int remaining)
    {
        if (cache.TryGetValue((number, remaining), out long result)) return result;
        if (remaining == 0) return 1;

        if (number == 0)
        {
            cache[(number, remaining)] = DepthFirstSearch(1, remaining - 1); 
            return cache[(number, remaining)];
        }
        
        int digits = CountDigits(number);
        if (digits % 2 == 0)
        {
            (long left, long right) = SplitNumber(number, digits/2);
            cache[(number, remaining)] = DepthFirstSearch(left, remaining-1) + DepthFirstSearch(right, remaining - 1);
            return cache[(number, remaining)];
        }

        cache[(number, remaining)] = DepthFirstSearch(number * 2024, remaining - 1);
        return cache[(number, remaining)];
    }

    public override long Part2() => startingNumbers.Sum(n => DepthFirstSearch(n, 75));
}