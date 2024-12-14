using System.Text;
using System.Text.RegularExpressions;

namespace aoc_2024.Days;

public class Day14 : BaseDay
{
    private readonly record struct Point(int X, int Y)
    {
        public static Point operator+(Point p1, Point p2) => new(p1.X + p2.X, p1.Y + p2.Y);
        public static Point operator*(Point p, int x) => new(p.X * x, p.Y * x);

        public static Point Parse(string s)
        {
            var nums = Regex.Matches(s, @"-?\d+");
            return nums switch
            {
                [var a, var b] => new Point(int.Parse(a.Value), int.Parse(b.Value)),
                _ => throw new FormatException()
            };
        }
    }

    private readonly record struct Rect(Point Pos, int Width, int Height)
    {
        public bool Contains(Point point) => point.X >= Pos.X && point.Y >= Pos.Y && point.X <= Pos.X+ Width && point.Y <= Pos.Y + Height;
    }

    private class Robot(Point position, Point velocity)
    {
        private readonly Point initialPosition = position;
        public Point Position { get; private set; } = position;
        private Point Velocity { get; } = velocity;

        public void Update()
        {
            int newX = (Position.X + Velocity.X + Width) % Width;
            int newY = (Position.Y + Velocity.Y + Height) % Height;
            Position = new Point(newX, newY);
        }

        public void Reset()
        {
            Position = initialPosition;
        }
    }

    private const int Width = 101;
    private const int Height = 103;
    private static readonly bool drawTree = false;
    
    
    private readonly List<Robot> robots;
    
    private static readonly Rect NorthWest = new(new Point(0, 0), Width/2-1, Height/2-1);
    private static readonly Rect NorthEast = new(new Point(Width/2+1, 0), Width/2-1, Height/2-1);
    private static readonly Rect SouthWest = new(new Point(0, Height/2+1), Width/2-1, Height/2-1);
    private static readonly Rect SouthEast = new(new Point(Width/2+1, Height/2+1), Width/2-1, Height/2-1);

    public Day14()
    {
        robots = [];

        foreach (string line in Input.Split(Environment.NewLine))
        {
            string[] parts = line.Split(" ");

            var position = Point.Parse(parts[0]);
            var velocity = Point.Parse(parts[1]);
            robots.Add(new Robot(position, velocity));
        }
    }
    
    
    
    public override long Part1()
    {
        for (int i = 0; i < 100; i++)
        {
            foreach (var robot in robots) robot.Update();
        }
        
        return Score();
    }

    private long Score()
    {
        long nw = 0;
        long ne = 0;
        long sw = 0;
        long se = 0;
        foreach (var robot in robots)
        {
            if (NorthWest.Contains(robot.Position)) nw++;
            if (NorthEast.Contains(robot.Position)) ne++;
            if (SouthWest.Contains(robot.Position)) sw++;
            if (SouthEast.Contains(robot.Position)) se++;
        }
        
        return nw * ne * sw * se;
    }

    public override long Part2()
    {
        int seconds = 1;
        long minScore = int.MaxValue;
        int minSeconds = 1;
        string tree = "";
        
        foreach (var robot in robots) robot.Reset();

        while (seconds < 10_000)
        {
            
            foreach (var robot in robots) robot.Update();
            long score = Score();
            
            if (score < minScore)
            {
                minScore = score;
                minSeconds = seconds;
                tree = Print();

            }
            seconds++;
        }
        
        if (drawTree) Console.WriteLine(tree);
        return minSeconds;
    }

    private string Print()
    {
        char[,] grid = new char[Height, Width];
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                grid[y, x] = ' ';
            }
        }

        foreach (var robot in robots)
        {
            grid[robot.Position.Y, robot.Position.X] = 'x';
        }
        
        
        var sb = new StringBuilder();
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                sb.Append(grid[y, x]);
            }
            
            sb.Append(Environment.NewLine);
        }
        
        return sb.ToString();
    }
}