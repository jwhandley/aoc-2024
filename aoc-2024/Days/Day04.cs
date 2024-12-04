namespace aoc_2024.Days;

public class Day04 : BaseDay
{
    private readonly string[] lines;

    private static readonly (int dr, int dc)[] Directions =
    [
        (0, 1), // Right
        (0, -1), // Left
        (-1, 0), // Up
        (1, 0), // Down
        (-1, -1), // Up-Left
        (1, -1), // Down-Left
        (-1, 1), // Up-Right
        (1, 1) // Down-Right
    ];
    
    private static readonly char[] Target = ['X', 'M', 'A', 'S'];

    private static readonly string[] Patterns = [
        """
        M.M
        .A.
        S.S
        """,
        """
        M.S
        .A.
        M.S
        """,
        """
        S.S
        .A.
        M.M
        """,
        """
        S.M
        .A.
        S.M
        """
    ];

    public Day04()
    {
        lines = Input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    }

    public override long Part1()
    {
        int count = 0;
        for (int r = 0; r < lines.Length; r++)
        {
            for (int c = 0; c < lines[0].Length; c++)
            {
                if (lines[r][c] != 'X') continue;
                count += Directions.Count(dir => IsXmas(r, c, dir));
            }
        }


        return count;
    }

    private bool IsXmas(int r, int c, (int dr, int dc) dir)
    {
        for (int i = 0; i < 4; i++)
        {
            int x = c + dir.dc * i;
            int y = r + dir.dr * i;
            if (x < 0 || x >= lines.Length || y < 0 || y >= lines.Length) return false;
            if (lines[y][x] != Target[i]) return false;
        }

        return true;
    }

    private bool HasPattern(int r, int c, string[] pattern)
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (pattern[i][j] == '.') continue;
                int y = r + i;
                int x = c + j;
                if (x < 0 || x >= lines.Length || y < 0 || y >= lines.Length) return false;
                
                if (lines[y][x] != pattern[i][j]) return false;
            }
        }
        
        
        return true;
    }


    public override long Part2()
    {
        int count = 0;
        for (int r = 0; r < lines.Length; r++)
        {
            for (int c = 0; c < lines[0].Length; c++)
            {
                if (Patterns.Any(pattern => HasPattern(r, c, pattern.Split('\n'))))
                {
                    count++;
                }
            }
        }


        return count;
    }
}