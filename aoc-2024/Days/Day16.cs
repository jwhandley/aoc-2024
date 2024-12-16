namespace aoc_2024.Days;

using Graph = Dictionary<(int r, int c, Direction dir), HashSet<(int r, int c, Direction dir)>>;

public enum Direction
{
    North,
    South,
    East,
    West
}



public class Day16 : BaseDay
{
    private readonly char[,] grid;
    private readonly int width;
    private readonly int height;
    private readonly (int r, int c) startPos;
    private readonly (int r, int c) targetPos;

    public Day16()
    {
        width = Input.Split(Environment.NewLine).Length;
        height = Input.Split(Environment.NewLine)[0].Length;
        grid = new char[height, width];

        foreach ((int r, string line) in Input.Split(Environment.NewLine).Index())
        {
            foreach ((int c, char ch) in line.Index())
            {
                grid[r, c] = ch;
                if (ch == 'S') startPos = (r, c);
                if (ch == 'E') targetPos = (r, c);
            }
        }
    }

    private int Dijkstra((int r, int c) start, (int r, int c) target)
    {
        Dictionary<(int r, int c, Direction dir), int> dist = [];

        var q = new PriorityQueue<(int r, int c, Direction, int d), int>();
        q.Enqueue((start.r, start.c, Direction.East, 0), 0);
        dist[(start.r, start.c, Direction.East)] = 0;

        while (q.Count > 0)
        {
            (int r, int c, var dir, int d) = q.Dequeue();

            if (!InBounds(r, c) || grid[r, c] == '#') continue;

            (int nr, int nc) = dir switch
            {
                Direction.North => (r - 1, c),
                Direction.South => (r + 1, c),
                Direction.East => (r, c + 1),
                Direction.West => (r, c - 1),
                _ => throw new ArgumentOutOfRangeException()
            };

            int distance = dist.GetValueOrDefault((nr, nc, dir), int.MaxValue);

            if (distance > d + 1)
            {
                dist[(nr, nc, dir)] = d + 1;
                q.Enqueue((nr, nc, dir, d + 1), d + 1);
            }


            var left = RotateLeft(dir);
            int leftDistance = dist.GetValueOrDefault((r, c, left), int.MaxValue);
            if (leftDistance > d + 1000)
            {
                dist[(r, c, left)] = d + 1000;
                q.Enqueue((r, c, left, d + 1000), d + 1000);
            }


            var right = RotateRight(dir);
            int rightDistance = dist.GetValueOrDefault((r, c, right), int.MaxValue);
            if (rightDistance > d + 1000)
            {
                dist[(r, c, right)] = d + 1000;
                q.Enqueue((r, c, right, d + 1000), d + 1000);
            }
        }

        return dist.Where(e => (e.Key.r, e.Key.c) == target).Min(e => e.Value);
    }

     

    private static Direction RotateLeft(Direction dir) => dir switch
    {
        Direction.North => Direction.West,
        Direction.South => Direction.East,
        Direction.East => Direction.North,
        Direction.West => Direction.South,
        _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
    };

    private static Direction RotateRight(Direction dir) => dir switch
    {
        Direction.North => Direction.East,
        Direction.South => Direction.West,
        Direction.East => Direction.South,
        Direction.West => Direction.North,
        _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
    };


    private bool InBounds(int r, int c) => r >= 0 && r < height && c >= 0 && c < width;

    private int Test((int r, int c) start, (int r, int c) target)
    {
        Dictionary<(int r, int c, Direction dir), int> dist = [];
        Dictionary<(int r, int c, Direction dir), HashSet<(int r, int c, Direction dir)>> prev = [];
        for (int r = 0; r < height; r++)
        {
            for (int c = 0; c < width; c++)
            {
                
                prev[(r, c, Direction.North)] = [];
                prev[(r, c, Direction.South)] = [];
                prev[(r, c, Direction.East)] = [];
                prev[(r, c, Direction.West)] = [];
            }
        }

        var q = new PriorityQueue<(int r, int c, Direction, int d), int>();
        q.Enqueue((start.r, start.c, Direction.East, 0), 0);
        dist[(start.r, start.c, Direction.East)] = 0;

        while (q.Count > 0)
        {
            (int r, int c, var dir, int d) = q.Dequeue();

            if (!InBounds(r, c) || grid[r, c] == '#') continue;

            (int nr, int nc) = dir switch
            {
                Direction.North => (r - 1, c),
                Direction.South => (r + 1, c),
                Direction.East => (r, c + 1),
                Direction.West => (r, c - 1),
                _ => throw new ArgumentOutOfRangeException()
            };

            int distance = dist.GetValueOrDefault((nr, nc, dir), int.MaxValue);

            if (distance > d + 1)
            {
                prev[(nr, nc, dir)] = [(r, c, dir)];
                dist[(nr, nc, dir)] = d + 1;
                q.Enqueue((nr, nc, dir, d + 1), d + 1);
            } 
            else if (distance == d + 1)
            {
                prev[(nr, nc, dir)].Add((r, c, dir));
                q.Enqueue((nr, nc, dir, d + 1), d + 1);
            }
             


            var left = RotateLeft(dir);
            int leftDistance = dist.GetValueOrDefault((r, c, left), int.MaxValue);
            if (leftDistance > d + 1000)
            {
                prev[(r, c, left)] = [(r, c, dir)];
                dist[(r, c, left)] = d + 1000;
                q.Enqueue((r, c, left, d + 1000), d + 1000);
            }
            else if (leftDistance == d + 1000)
            {
                prev[(r, c, left)].Add((r, c, dir));
                q.Enqueue((r, c, left, d + 1000), d + 1000);
            }


            var right = RotateRight(dir);
            int rightDistance = dist.GetValueOrDefault((r, c, right), int.MaxValue);
            if (rightDistance > d + 1000)
            {
                prev[(r, c, right)] = [(r, c, dir)];
                dist[(r, c, right)] = d + 1000;
                q.Enqueue((r, c, right, d + 1000), d + 1000);
            }
            else if (rightDistance == d + 1000)
            {
                prev[(r, c, right)].Add((r, c, dir));
                q.Enqueue((r, c, right, d + 1000), d + 1000);
            }
        }
        
        return CheckPaths(prev, (target.r, target.c, Direction.East), (start.r, start.c, Direction.East));
    }

    private int CheckPaths(Graph graph, (int r, int c, Direction dir) start, (int r, int c, Direction dir) target)
    {
        HashSet<(int r, int c)> seen = [];
        List<(int r, int c, Direction dir)> path = [start];
        Queue<List<(int r, int c, Direction dir)>> queue = [];
        queue.Enqueue(path);

        while (queue.Count > 0)
        {
            path = queue.Dequeue();
            var last = path.Last();

            if (last == target)
            {
                foreach ((int r, int c, _) in path)
                {
                    seen.Add((r, c));
                }

                continue;
            }

            foreach (var nbr in graph[last])
            {
                if (path.Contains(nbr)) continue;
                List<(int r, int c, Direction dir)> newPath = [..path, nbr];
                queue.Enqueue(newPath);
            }
        }

        return seen.Count;
    }
    

    public override long Part1()
    {
        return Dijkstra(startPos, targetPos);
    }

    public override long Part2() => Test(startPos, targetPos);
}