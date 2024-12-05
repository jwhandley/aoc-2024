namespace aoc_2024.Days;

public class Day05 : BaseDay
{
    private readonly IReadOnlyCollection<List<int>> updates;

    private readonly HashSet<(int lo, int hi)> ruleSet;

    public Day05()
    {
        string[] parts = Input.Split("\n\n");

        ruleSet = parts[0].Split('\n')
            .Select(l => l.Split('|'))
            .Select(rule => (int.Parse(rule[0]), int.Parse(rule[1])))
            .ToHashSet();


        updates = parts[1].Split('\n')
            .Select(l => l.Split(',')
                .Select(int.Parse)
                .ToList())
            .ToList();
    }

    public override long Part1() => updates.Where(IsSorted).Sum(update => update[update.Count / 2]);

    private bool IsSorted(List<int> update)
    {
        for (int i = 1; i < update.Count; i++)
        {
            if (ruleSet.Contains((update[i], update[i - 1])))
                return false;
        }

        return true;
    }

    private int Compare(int i, int j)
    {
        if (ruleSet.Contains((i, j))) return -1;
        return ruleSet.Contains((j, i)) ? 1 : 0;
    }

    public override long Part2() => updates.Where(u => !IsSorted(u)).Sum(u =>
    {
        u.Sort(Compare);
        return u[u.Count / 2];
    });
}