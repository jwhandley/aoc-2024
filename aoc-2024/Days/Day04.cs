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
        for (int i = 1; i < 4; i++)
        {
            int x = c + dir.dc * i;
            int y = r + dir.dr * i;
            if (x < 0 || x >= lines.Length || y < 0 || y >= lines.Length) return false;
            if (lines[y][x] != Target[i]) return false;
        }

        return true;
    }

    public override long Part2()
    {
        int count = 0;
        for (int r = 1; r < lines.Length - 1; r++)
        {
            for (int c = 1; c < lines[0].Length - 1; c++)
            {
                if (lines[r][c] != 'A') continue;
                if (lines[r - 1][c + 1] == 'M' && lines[r - 1][c - 1] == 'M' && lines[r + 1][c + 1] == 'S' &&
                    lines[r + 1][c - 1] == 'S') count++;
                if (lines[r - 1][c + 1] == 'S' && lines[r - 1][c - 1] == 'S' && lines[r + 1][c + 1] == 'M' &&
                    lines[r + 1][c - 1] == 'M') count++;
                if (lines[r - 1][c + 1] == 'M' && lines[r - 1][c - 1] == 'S' && lines[r + 1][c + 1] == 'M' &&
                    lines[r + 1][c - 1] == 'S') count++;
                if (lines[r - 1][c + 1] == 'S' && lines[r - 1][c - 1] == 'M' && lines[r + 1][c + 1] == 'S' &&
                    lines[r + 1][c - 1] == 'M') count++;
            }
        }


        return count;
    }
}