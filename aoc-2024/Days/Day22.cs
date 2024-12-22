namespace aoc_2024.Days;

public class Day22 : BaseDay
{
    private readonly int[] seeds;
    private readonly Dictionary<int, int> prices;

    public Day22()
    {
        prices = [];
        seeds = Input.Split('\n').Select(int.Parse).ToArray();
    }


    private static long Next(long seed)
    {
        long step1 = seed << 6;
        seed ^= step1;
        seed &= 0xFFFFFF;
        
        long step2 = seed >> 5;
        seed ^= step2;
        seed &= 0xFFFFFF;

        long step3 = seed << 11;
        seed ^= step3;
        seed &= 0xFFFFFF;

        return seed;
    }

    private static long Repeat(long number, int repeats)
    {
        for (int i = 0; i < repeats; ++i)
        {
            number = Next(number);
        }

        return number;
    }

    private void Sequence(long number, int repeats)
    {
        List<int> numbers = [(int)(number % 10)];
        for (int i = 0; i < repeats; ++i)
        {
            number = Next(number);
            numbers.Add((int)(number % 10));
        }
        
        
        HashSet<int> seen = [];
        for (int i = 4; i < numbers.Count; ++i)
        {
            int d1 = numbers[i - 3] - numbers[i - 4];
            int d2 = numbers[i - 2] - numbers[i - 3];
            int d3 = numbers[i - 1] - numbers[i - 2];
            int d4 = numbers[i] - numbers[i - 1];

            int hash = ((d1 + 9) << 15)|((d2 +9) << 10)|((d3 + 9) << 5)|(d4 + 9);

            if (!seen.Add(hash)) continue;
            if (!prices.TryGetValue(hash, out int value))
            {
                prices[hash] = numbers[i];
            }
            prices[hash] = value + numbers[i];
        }
    }

    public override string Part1() => seeds.Select(s => Repeat(s, 2000)).Sum().ToString();
    public override string Part2()
    {
        foreach (int seed in seeds)
        {
            Sequence(seed, 2000);
        }
        
        return prices.MaxBy(r => r.Value).Value.ToString();
    }
}