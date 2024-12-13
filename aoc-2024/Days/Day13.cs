using System.Text.RegularExpressions;

namespace aoc_2024.Days;

public partial class Day13 : BaseDay
{
    private readonly List<(Point prize, Point buttonA, Point buttonB)> machines = [];

    public Day13()
    {
        string[] groups = Input.Split("\n\n");
        
        foreach (string group in groups)
        {
            string[] parts = group.Split(Environment.NewLine);
            var buttonA = Point.Parse(parts[0]);
            var buttonB = Point.Parse(parts[1]);
            var prize = Point.Parse(parts[2]);
            
            machines.Add((prize, buttonA, buttonB));
        }
    }

    public override long Part1()
    {
        long result = 0;
        foreach (var (prize, buttonA, buttonB) in machines)
        {
            long determinant = buttonA.X * buttonB.Y - buttonB.X * buttonA.Y; 
            
            long x1 = buttonB.Y * prize.X - buttonB.X * prize.Y;
            long x2 = -buttonA.Y * prize.X + buttonA.X * prize.Y;
            
            if (x1 % determinant != 0 || x2 % determinant != 0) continue;
            result += x1/determinant*3 + x2/determinant;
        }
        
        return result;
    }

    private readonly partial record struct Point(long X, long Y)
    {
        public static Point operator+(Point p1, Point p2) => new(p1.X + p2.X, p1.Y + p2.Y);
        public static Point operator-(Point p1, Point p2) => new(p1.X - p2.X, p1.Y - p2.Y);
        public static Point operator*(Point p, long count) => new(p.X * count, p.Y * count);
        public static Point Parse(string input)
        {
            int[] nums = NumRegex().Matches(input).Select(m => int.Parse(m.Value)).ToArray();
            return new Point(nums[0], nums[1]);
        }

        [GeneratedRegex(@"\d+")]
        private static partial Regex NumRegex();
    }

    public override long Part2()
    {
        long result = 0;
        foreach (var (prize, buttonA, buttonB) in machines)
        {
            var target = new Point(prize.X + 10000000000000, prize.Y + 10000000000000);
            long determinant = buttonA.X * buttonB.Y - buttonB.X * buttonA.Y; 
            
            long x1 = buttonB.Y * target.X - buttonB.X * target.Y;
            long x2 = -buttonA.Y * target.X + buttonA.X * target.Y;
            
            if (x1 % determinant != 0 || x2 % determinant != 0) continue;
            result += x1/determinant*3 + x2/determinant;
        }
        
        return result;
    }
}