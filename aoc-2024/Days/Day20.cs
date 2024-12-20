using System.Numerics;

namespace aoc_2024.Days;

public class Day20 : BaseDay
{
    private readonly Vector2 start;
    private readonly Vector2 end;
    private readonly HashSet<Vector2> walls;
    private readonly List<Vector2> path;
    private readonly int width;
    private readonly int height;

    private readonly Vector2[] directions =
    [
        Vector2.UnitX,
        Vector2.UnitY,
        -Vector2.UnitX,
        -Vector2.UnitY
    ];

    public Day20()
    {
        walls = new HashSet<Vector2>();
        height = Input.Split('\n').Length;
        width = Input.Split('\n')[0].Length;
        foreach (var (r, line) in Input.Split(Environment.NewLine).Index())
        {
            foreach (var (c, ch) in line.Index())
            {
                if (ch == '#') walls.Add(new Vector2(c, r));
                if (ch == 'S') start = new Vector2(c, r);
                if (ch == 'E') end = new Vector2(c, r);
            }
        }

        path = BreadthFirstSearch();
    }

    private List<Vector2> BreadthFirstSearch()
    {
        Queue<Vector2> queue = [];
        queue.Enqueue(start);
        List<Vector2> found = [];
        HashSet<Vector2> visited = [];

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (!visited.Add(current)) continue;
            found.Add(current);

            if (current == end) return found;

            foreach (var dir in directions)
            {
                if (!walls.Contains(current + dir) && InBounds(current + dir))
                {
                    queue.Enqueue(current + dir);
                }
            }
        }

        return found;
    }

    private bool InBounds(Vector2 point) => point.X >= 0 && point.X < width && point.Y >= 0 && point.Y < height;
    private static int ManhattanDist(Vector2 a, Vector2 b) => (int)Math.Abs(a.X - b.X) + (int)Math.Abs(a.Y - b.Y);

    public override long Part1()
    {
        Dictionary<Vector2, int> visited = path.Index().ToDictionary(p => p.Item, p => p.Index);

        int cost = path.Count;
        int result = 0;

        for (int i = 0; i < path.Count - 1; i++)
        {
            for (int dx = -2; dx <= 2; dx++)
            {
                for (int dy = -2; dy <= 2; dy++)
                {
                    var other = path[i] + new Vector2(dx, dy);
                    if (!visited.TryGetValue(other, out int j)) continue;
                    int distance = ManhattanDist(path[i], other);
                    if (distance != 2) continue;
                    int newCost = i + cost - j + distance;
                    if (cost - newCost >= 100) result++;
                }
            }
        }

        return result;
    }

    public override long Part2()
    {
        int cost = path.Count;
        int result = 0;

        for (int i = 0; i < path.Count - 1; i++)
        {
            for (int j = i + 1; j < path.Count; j++)
            {
                int dist = ManhattanDist(path[i], path[j]);
                if (dist > 20) continue;
                int newCost = i + cost - j + dist;
                if (cost - newCost >= 100) result++;
            }
        }

        return result;
    }
}