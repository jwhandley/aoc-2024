namespace aoc_2024.Days;

public class Day19 : BaseDay
{
    private readonly List<string> patterns;
    private readonly List<string> targets;
    private readonly Dictionary<string, long> cache = [];

    public Day19()
    {
        string[] parts = Input.Split("\n\n");

        patterns = parts[0].Split(", ").ToList();
        targets = parts[1].Split("\n").ToList();
    }

    public override long Part1() => targets.Count(CanMake);


    private bool CanMake(string target) =>
        target == "" || patterns.Where(target.StartsWith).Any(p => CanMake(target[p.Length..]));


    private long MakeCount(string target)
    {
        if (cache.TryGetValue(target, out long count)) return count;

        if (target == string.Empty) return 1;
        long result = patterns.Where(target.StartsWith).Sum(p => MakeCount(target[p.Length..]));

        cache[target] = result;
        return result;
    }

    public override long Part2() => targets.Sum(MakeCount);
}