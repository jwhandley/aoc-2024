namespace aoc_2024.Days;

public enum Direction
{
    North,
    South,
    East,
    West
}

public readonly record struct State(int R, int C, Direction Dir);

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
        Dictionary<State, int> dist = [];

        var q = new PriorityQueue<(State, int cost), int>();
        var state = new State(start.r, start.c, Direction.East);
        q.Enqueue((state, 0), 0);
        dist[state] = 0;

        while (q.Count > 0)
        {
            (var current, int cost) = q.Dequeue();

            if ((current.R, current.C) == target)
            {
                return cost;
            }

            if (!InBounds(current.R, current.C) || grid[current.R, current.C] == '#') continue;
            
            int distance = dist.GetValueOrDefault(current, int.MaxValue);
            if (distance < cost) continue;
            dist[current] = cost;

            (int nr, int nc) = current.Dir switch
            {
                Direction.North => (current.R - 1, current.C),
                Direction.South => (current.R + 1, current.C),
                Direction.East => (current.R, current.C + 1),
                Direction.West => (current.R, current.C - 1),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            q.Enqueue((current with {R = nr, C = nc}, cost + 1), cost + 1);
            
            var left = RotateLeft(current.Dir);
            q.Enqueue((current with {Dir = left}, cost + 1000), cost + 1000);
            
            var right = RotateRight(current.Dir);
            q.Enqueue((current with {Dir = right}, cost + 1000), cost + 1000);
        }
        
        throw new Exception("No paths found");
    }
    
    private int ShortestPaths((int r, int c) start, (int r, int c) target)
    {
        Dictionary<State, int> dist = [];
        Dictionary<State, HashSet<State>> prev = [];
        var q = new PriorityQueue<(State curr, State? prev, int cost), int>();
        var state = new State(start.r, start.c, Direction.East);
        q.Enqueue((state, null, 0), 0);
        dist[state] = 0;

        while (q.Count > 0)
        {
            (var current, State? previous, int cost) = q.Dequeue();
            if (!InBounds(current.R, current.C) || grid[current.R, current.C] == '#') continue;
            
            int distance = dist.GetValueOrDefault(current, int.MaxValue);
            if (distance < cost) continue;
            dist[current] = cost;
            
            if (!prev.ContainsKey(current)) prev[current] = [];
            if (previous is { } previousState)
            {
                prev[current].Add(previousState);    
            }

            (int nr, int nc) = current.Dir switch
            {
                Direction.North => (current.R - 1, current.C),
                Direction.South => (current.R + 1, current.C),
                Direction.East => (current.R, current.C + 1),
                Direction.West => (current.R, current.C - 1),
                _ => throw new ArgumentOutOfRangeException()
            };
            
            q.Enqueue((current with {R = nr, C = nc}, current, cost + 1), cost + 1);
            
            var left = RotateLeft(current.Dir);
            q.Enqueue((current with {Dir = left}, current, cost + 1000), cost + 1000);
            
            var right = RotateRight(current.Dir);
            q.Enqueue((current with {Dir = right}, current, cost + 1000), cost + 1000);
        }

        return CheckPaths(prev, new State(target.r, target.c, Direction.East), new State(start.r, start.c, Direction.East));
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
    
    
    private static int CheckPaths(Dictionary<State, HashSet<State>> graph, State start, State target)
    {
        HashSet<(int r, int c)> seen = [];
        List<State> path = [start];
        Queue<List<State>> queue = [];
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
                List<State> newPath = [..path, nbr];
                queue.Enqueue(newPath);
            }
        }

        return seen.Count;
    }


    public override string Part1() => Dijkstra(startPos, targetPos).ToString(); 

    public override string Part2() => ShortestPaths(startPos, targetPos).ToString();
}