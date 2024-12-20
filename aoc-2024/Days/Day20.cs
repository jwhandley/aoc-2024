using System.Numerics;

namespace aoc_2024.Days;

public class Day20 : BaseDay
{
    private readonly Vector2 start;
    private readonly Vector2 end;
    private readonly HashSet<Vector2> walls;
    private readonly Dictionary<Vector2, int> path;
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
        foreach ((int r, string line) in Input.Split(Environment.NewLine).Index())
        {
            foreach ((int c, char ch) in line.Index())
            {
                if (ch == '#') walls.Add(new Vector2(c, r));
                if (ch == 'S') start = new Vector2(c, r);
                if (ch == 'E') end = new Vector2(c, r);
            }
        }

        path = GetPath();
    }

    private Dictionary<Vector2, int> GetPath()
    {
        Dictionary<Vector2, int> distances = [];
        distances[start] = 0;
        var current = start;

        while (current != end)
        {
            var dir = directions.First(d =>
                !walls.Contains(current + d)
                && InBounds(current + d)
                && !distances.ContainsKey(current + d)
            );
            
            var next = current + dir;
            if (distances.ContainsKey(next)) continue;
            distances[next] = distances[current] + 1;
            current = next;
        }

        return distances;
    }

    private bool InBounds(Vector2 point) => point.X >= 0 && point.X < width && point.Y >= 0 && point.Y < height;
    private static int ManhattanDist(Vector2 a, Vector2 b) => (int)Math.Abs(a.X - b.X) + (int)Math.Abs(a.Y - b.Y);

    private int Solve(int maxDistance)
    {
        int result = 0;

        foreach ((var point, int dist) in path)
        {
            for (int dx = -maxDistance; dx <= maxDistance; dx++)
            {
                for (int dy = -maxDistance; dy <= maxDistance; dy++)
                {
                    var other = point + new Vector2(dx, dy);
                    if (!path.TryGetValue(other, out int j)) continue;
                    int distance = ManhattanDist(point, other);
                    if (distance < 2 || distance > maxDistance) continue;
                    if (j - dist >= 100 + distance) result++;
                }
            }
        }

        return result;
    }

    public override long Part1() => Solve(2);
    public override long Part2() => Solve(20);
}