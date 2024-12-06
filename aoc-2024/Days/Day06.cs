namespace aoc_2024.Days;

public class Day06 : BaseDay
{
    private readonly string[] grid;
    
    private readonly (int r, int c) guardStart;

    public Day06()
    {
        grid = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        guardStart = FindGuard();
    }

    private (int r, int c) FindGuard()
    {
        for (int y = 0; y < grid.Length; y++)
        {
            for (int x = 0; x < grid[y].Length; x++)
            {
                if (grid[y][x] == '^') return (y, x);
            }
        }
        
        return (0, 0);
    }
    
    private bool InBounds(int r, int c) => r >= 0 && r < grid.Length && c >= 0 && c < grid[0].Length;
    public override long Part1() => Path().Count;

    private HashSet<(int x, int y)> Path()
    {
        var guard = guardStart;
        (int dr, int dc) = (-1, 0);
        HashSet<(int x, int y)> visited = [];

        while (true)
        {
            visited.Add(guard);
            if (!InBounds(guard.r + dr, guard.c + dc)) break;

            if (grid[guard.r + dr][guard.c + dc] == '#')
            {
                (dr, dc) = (dc, -dr);
            }
            
            guard = (guard.r + dr, guard.c + dc);    
            
        }
        
        return visited;
    }

    private bool InLoop((int r, int c) obstruction)
    {
        
        HashSet<(int x, int y, int dr, int dc)> visited = [];
        var guard = guardStart;
        (int dr, int dc) = (-1, 0);

        while (true)
        {
            
            if (!InBounds(guard.r + dr, guard.c + dc)) return false;

            if (grid[guard.r + dr][guard.c + dc] == '#' || (guard.r + dr, guard.c + dc) == obstruction)
            {
                if (!visited.Add((guard.r, guard.c, dr, dc))) return true;
                (dr, dc) = (dc, -dr);
            }

            guard = (guard.r + dr, guard.c + dc);
        }
    }

    public override long Part2() => Path().Where(pos => pos != guardStart).Count(InLoop);
    
}