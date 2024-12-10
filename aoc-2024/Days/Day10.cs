namespace aoc_2024.Days;

public class Day10 : BaseDay
{
    private int[,] Grid { get; }
    private List<(int r, int c)> TrailHeads { get; }
    private int Width { get; }
    private int Height { get; }

    private static readonly (int dr, int dc)[] Dirs =
    [
        (-1, 0), // Up
        (1, 0), // Down
        (0, -1), // Left
        (0, 1) // Right
    ];

    public Day10()
    {
        TrailHeads = [];
        Height = Input.Length;
        Width = Input.Split('\n')[0].Length;
        Grid = new int[Height, Width];
        foreach ((int r, string line) in Input.Split(Environment.NewLine).Index())
        {
            foreach ((int c, char ch) in line.Index())
            {
                if (ch == '0') TrailHeads.Add((r, c));
                Grid[r, c] = ch - '0';
            }
        }
    }
    public override long Part1() => TrailHeads.Sum(head => BreadthFirstSearch(head.r, head.c));

    private int BreadthFirstSearch(int startRow, int startCol)
    {
        Queue<(int r, int c)> queue = new();
        queue.Enqueue((startRow, startCol));
        
        
        HashSet<(int r, int c)> visited = [];
        while (queue.Count > 0)
        {
            (int r, int c) = queue.Dequeue();
            
            if (!visited.Add((r,c))) continue;

            foreach ((int dr, int dc) in Dirs)
            {
                if (!InBounds(r + dr, c + dc) || Grid[r+dr,c+dc] - Grid[r,c] != 1) continue;
                
                queue.Enqueue((r + dr, c + dc));
            }
        }
        
        return visited.Count(p => Grid[p.r, p.c] == 9);
    }
    
    private int BreadthFirstSearch2(int startRow, int startCol)
    {
        Queue<(int r, int c)> queue = new();
        queue.Enqueue((startRow, startCol));
        
        int count = 0;
        while (queue.Count > 0)
        {
            (int r, int c) = queue.Dequeue();
            if (Grid[r, c] == 9) count++;

            foreach ((int dr, int dc) in Dirs)
            {
                if (!InBounds(r + dr, c + dc) || Grid[r+dr,c+dc] - Grid[r,c] != 1) continue;
                
                queue.Enqueue((r + dr, c + dc));
            }
        }
        
        return count;
    }
    
    private bool InBounds(int r, int c) => r >= 0 && r < Height && c >= 0 && c < Width;
    public override long Part2() => TrailHeads.Sum(head => BreadthFirstSearch2(head.r, head.c));
}