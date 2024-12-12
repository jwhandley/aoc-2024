namespace aoc_2024.Days;

public class Day12 : BaseDay
{
    private char[,] Grid { get; }
    
    public Day12()
    {
        int rows = Input.Split(Environment.NewLine).Length;
        int cols = Input.Split(Environment.NewLine)[0].Length;
        Grid = new char[rows, cols];
        foreach ((int row, string line) in Input.Split(Environment.NewLine).Index())
        {
            foreach ((int col, char c) in line.Index())
            {
                Grid[row, col] = c;
            }
        }
    }

    private readonly struct Point(int row, int col)
    {
        public int Row { get; } = row;
        public int Col { get; } = col;
        
        public static Point operator+(Point p1, Point p2) => new(p1.Row + p2.Row, p1.Col + p2.Col);
        
        public static readonly Point Down = new(1, 0);
        public static readonly Point Left = new(0, -1);
        public static readonly Point Right = new(0, 1);
        public static readonly Point Up = new(-1, 0);

        public static IEnumerable<Point> Dirs()
        {
            yield return Up;
            yield return Down;
            yield return Left;
            yield return Right;
        }
    }

    private readonly struct Region(List<Point> points, HashSet<(Point p1, Point p2)> boundary)
    {
        private List<Point> Points { get; } = points;
        private HashSet<(Point p1, Point p2)> Boundary { get; } = boundary;

        public int Area() => Points.Count;
        public int Perimeter() => Boundary.Count;

        public int Sides()
        {
            int count = 0;
            
            foreach (var (p1, p2) in Boundary)
            {
                if (Boundary.Contains((p1 + Point.Up, p2 + Point.Up)))
                {
                    continue;
                }
                
                if (Boundary.Contains((p1 + Point.Right, p2 + Point.Right)))
                {
                    continue;
                }

                count++;
            }

            return count;
        }
    }
    
    private bool InBounds(Point p) => p is { Row: >= 0, Col: >= 0 } && p.Row < Grid.GetLength(0) && p.Col < Grid.GetLength(1);

    private List<Region> FloodFill()
    {
        bool[,] visited = new bool[Grid.GetLength(0), Grid.GetLength(1)];
        List<Region> regions = [];

        for (int r = 0; r < Grid.GetLength(0); r++)
        {
            for (int c = 0; c < Grid.GetLength(1); c++)
            {
                if (visited[r, c]) continue;
                char type = Grid[r, c];
                
                Queue<Point> queue = [];
                queue.Enqueue(new Point(r, c));
                List<Point> seen = [];
                HashSet<(Point p1, Point p2)> boundaries = [];

                while (queue.Count > 0)
                {
                    var current = queue.Dequeue();
                    
                    if (!InBounds(current)) continue;
                    if (visited[current.Row, current.Col]) continue;
                    if (Grid[current.Row, current.Col] != type) continue;
                    
                    visited[current.Row, current.Col] = true;


                    foreach (Point dir in Point.Dirs())
                    {
                        if (!InBounds(current + dir) ||
                            Grid[current.Row + dir.Row, current.Col + dir.Col] != type)
                        {
                            boundaries.Add((current, current + dir));
                            
                        }
                        queue.Enqueue(current + dir);
                    }
                    seen.Add(current);
                }
                
                regions.Add(new Region(seen, boundaries));
            }
        }
        
        return regions;
    }

    
    public override long Part1() => FloodFill().Sum(r => r.Area() * r.Perimeter());

    public override long Part2() => FloodFill().Sum(r => r.Area() * r.Sides());
}