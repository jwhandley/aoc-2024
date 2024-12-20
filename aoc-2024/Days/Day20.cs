using System.Numerics;

namespace aoc_2024.Days;

public class Day20 : BaseDay
{
    private readonly Vector2 start;
    private Vector2 end;
    private readonly HashSet<Vector2> walls;
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
    }

    private Dictionary<Vector2, int> BreadthFirstSearch()
    {
        Queue<(Vector2, int d)> queue = new();
        queue.Enqueue((start, 0));
        Dictionary<Vector2, int> visited = [];

        while (queue.Count > 0)
        {
            var (current, dist) = queue.Dequeue();
            if (!visited.TryAdd(current, dist)) continue;

            if (current == end) return visited;

            foreach (var dir in directions)
            {
                if (!walls.Contains(current + dir) && InBounds(current + dir))
                {
                    queue.Enqueue((current + dir, dist + 1));
                }
            }
        }

        return visited;
    }

    private bool InBounds(Vector2 point) => point.X >= 0 && point.X < width && point.Y >= 0 && point.Y < height;
    private int ManhattanDist(Vector2 a, Vector2 b) => (int)Math.Abs(a.X - b.X) + (int)Math.Abs(a.Y - b.Y);

    public override long Part1()
    {
        Dictionary<Vector2, int> path = BreadthFirstSearch();
        int cost = path[end];
        int result = 0;

        foreach ((var startCheat, int dist1) in path)
        {
            for (int dx = -2; dx <= 2; dx++)
            {
                for (int dy = -2; dy <= 2; dy++)
                {
                    if (ManhattanDist(startCheat, startCheat + new Vector2(dx, dy)) != 2) continue;
                    if (!path.TryGetValue(startCheat + new Vector2(dx, dy), out int dist2)) continue;
                    int newCost = dist1 + (cost - dist2) + 2;
                    if (cost - newCost >= 100) result++;
                }
            }
        }

        return result;
    }

    public override long Part2()
    {
        Dictionary<Vector2, int> path = BreadthFirstSearch();
        int cost = path[end];
        int result = 0;
        foreach ((var startCheat, int dist1) in path)
        {
            foreach ((var endCheat, int dist2) in path)
            {
                if (ManhattanDist(startCheat, endCheat) > 20) continue;
                int manhattan = ManhattanDist(startCheat, endCheat);
                int newCost = dist1 + (cost - dist2) + manhattan;
                if (cost - newCost >= 100) result++;
            }
        }

        return result;
    }
}