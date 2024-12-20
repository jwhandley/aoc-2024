namespace aoc_2024.Days;

public class Day18 : BaseDay
{
    private readonly List<(int x, int y)> barriers;

    private readonly (int dx, int dy)[] directions =
    [
        (-1, 0),
        (1, 0),
        (0, -1),
        (0, 1)
    ];

    public Day18()
    {
        barriers = [];
        foreach (var line in Input.Split(Environment.NewLine))
        {
            int[] coords = line.Split(',').Select(int.Parse).ToArray();
            barriers.Add((coords[0], coords[1]));
        }
    }

    public override long Part1() => BreadthFirstSearch(0, 0, 70, 70, barriers.Take(1024).ToHashSet()) ?? 0;

    private int? BreadthFirstSearch(int x, int y, int tx, int ty, HashSet<(int x, int y)> fallen)
    {
        Queue<(int x, int y, int d)> queue = [];
        HashSet<(int x, int y)> visited = [];
        queue.Enqueue((x, y, 0));

        while (queue.Count > 0)
        {
            (int cx, int cy, int d) = queue.Dequeue();
            if (cx < 0 || cx > tx || cy < 0 || cy > ty || fallen.Contains((cx, cy))) continue;
            if (!visited.Add((cx, cy))) continue;
            if (cx == tx && cy == ty) return d;

            foreach ((int dx, int dy) in directions)
            {
                queue.Enqueue((cx + dx, cy + dy, d + 1));
            }
        }

        return null;
    }

    private int BinarySearch()
    {
        int low = 0;
        int high = barriers.Count - 1;

        while (low < high)
        {
            int mid = low + (high - low) / 2;
            HashSet<(int x, int y)> fallen = barriers.Take(mid + 1).ToHashSet();

            if (BreadthFirstSearch(0, 0, 70, 70, fallen) != null)
            {
                low = mid + 1;
            }
            else
            {
                high = mid;
            }
        }

        return low;
    }

    public override long Part2()
    {
        int idx = BinarySearch();

        Console.WriteLine(barriers[idx]);
        return idx;
    }
}