using System.Numerics;

namespace aoc_2024.Days;

public class Day15 : BaseDay
{
    private readonly char[,] grid;
    private readonly Vector2 startPos;
    private readonly List<Vector2> moves;

    public Day15()
    {
        string[] parts = Input.Split("\n\n");
            grid = new char[parts[0].Split('\n').Length, parts[0].Split('\n')[0].Length];
        foreach ((int r, string line) in parts[0].Split('\n').Index())
        {
            foreach ((int c, char ch) in line.Index())
            {
                grid[r, c] = ch;

                if (ch == '@') startPos = new Vector2(c, r);
            }
        }

        moves = [];
        foreach (char c in parts[1])
        {
            switch (c)
            {
                case '^':
                    
                    moves.Add(new Vector2(0, -1));
                    break;
                case 'v':
                    moves.Add(new Vector2(0, 1));
                    break;
                case '<':
                    moves.Add(new Vector2(-1, 0));
                    break;
                case '>':
                    moves.Add(new Vector2(1, 0));
                    break;
            }
        }
    }
    
    private IEnumerable<Body> LoadBodies(int width, char target)
    {
        for (int r = 0; r < grid.GetLength(0); r++)
        {
            for (int c = 0; c < grid.GetLength(1); c++)
            {
                if (grid[r, c] == target) yield return new Body(new Vector2(c*width, r), width, 1); 
            }
        }
    }

    public override long Part1()
    {
        var robot = new Body(startPos, 1, 1);
        List<Body> boxes = LoadBodies(1, 'O').ToList();
        List<Body> walls = LoadBodies(1, '#').ToList();

        foreach (var move in moves.Where(move => robot.CanMove(move, boxes, walls)))
        {
            robot.Move(move, boxes);
        }

        return CalculateScore(boxes);
    }
    
    

    
    public override long Part2()
    {
        var robot = new Body(startPos with { X = startPos.X * 2 }, 1, 1);
        List<Body> boxes = LoadBodies(2, 'O').ToList();
        List<Body> walls = LoadBodies(2, '#').ToList();

        foreach (var move in moves.Where(move => robot.CanMove(move, boxes, walls)))
        {
            robot.Move(move, boxes);
        }

        return CalculateScore(boxes);
    }
    
    private static long CalculateScore(IEnumerable<Body> boxes) => boxes.Sum(b => (int)b.Position.Y*100 + (int)b.Position.X);
}

public class Body(Vector2 pos, int width, int height)
{
    public Vector2 Position { get; private set; } = pos; 
    private int Width { get; } = width;
    private int Height { get; } = height;


    private bool Intersects(Body other)
    {
        if (Position.X + Width <= other.Position.X) return false;
        if (Position.Y + Height <= other.Position.Y) return false;
        if (other.Position.X + other.Width <= Position.X) return false;
        if (other.Position.Y + other.Height <= Position.Y) return false;

        return true;
    }

    public bool CanMove(Vector2 dir, List<Body> bodies, List<Body> walls)
    {
        var newRect = new Body(Position + dir, Width, Height);

        if (walls.Any(w => w.Intersects(newRect))) return false;
        foreach (var box in bodies.Where(b => b.Intersects(newRect) && b != this))
        {
            if (!box.CanMove(dir, bodies, walls)) return false;
        }

        return true;
    }

    public void Move(Vector2 dir, List<Body> bodies)
    {
        Position += dir;

        foreach (var body in bodies.Where(b => b != this && Intersects(b)))
        {
            body.Move(dir, bodies);
        }
    }

    public override string ToString() => $"({Position}, {Width}, {Height}), {GetHashCode()}";
}