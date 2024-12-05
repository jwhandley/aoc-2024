namespace aoc_2024.Days;

public class Day05 : BaseDay
{
    private readonly List<int[]> updates;
    private readonly Dictionary<int, List<int>> graph;

    public Day05()
    {
        string[] parts = Input.Split("\n\n");

        List<(int, int)> rules = parts[0]
            .Split('\n')
            .Select(l => l.Split('|')
                .Select(int.Parse)
                .ToArray())
            .Select(r => (r[0], r[1])).ToList();

        graph = [];
        foreach ((int lo, int hi) in rules)
        {
            if (!graph.ContainsKey(lo))
            {
                graph.Add(lo, []);
            }

            graph[lo].Add(hi);
        }

        updates = parts[1].Split('\n').Select(l => l.Split(',').Select(int.Parse).ToArray()).ToList();
    }

    public override long Part1() => updates.Where(IsSorted).Sum(update => update[update.Length / 2]);

    private bool IsSorted(int[] update)
    {
        for (int i = 0; i < update.Length; i++)
        {
            for (int j = i + 1; j < update.Length; j++)
            {
                if (!graph.TryGetValue(update[j], out List<int>? value) || !value.Contains(update[i])) continue;
                return false;
            }
        }

        return true;
    }

    private int Compare(int i, int j)
    {
        if (graph.TryGetValue(i, out List<int>? v1) && v1.Contains(j))
        {
            return -1;
        }

        if (graph.TryGetValue(j, out List<int>? v2) && v2.Contains(i))
        {
            return 1;
        }

        return 0;
    }

    public override long Part2() => updates.Where(u => !IsSorted(u)).Sum(u =>
    {
        Array.Sort(u, Compare);
        return u[u.Length / 2];
    });
}