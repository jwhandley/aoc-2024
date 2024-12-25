namespace aoc_2024.Days;

public class Day25 : BaseDay
{
    private readonly List<int[]> locks;
    private readonly List<int[]> keys;

    public Day25()
    {
        locks = [];
        keys = [];
        foreach (string grid in Input.Split("\n\n"))
        {
            string[] lines = grid.Split('\n');
            int rows = grid.Split("\n").Length;
            int cols = grid.Split("\n")[0].Length;

            int[] heights = new int[cols];


            bool isLock = grid.Split('\n')[0].All(c => c == '#');

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (lines[r][c] == '#') heights[c]++;
                }
            }

            for (int c = 0; c < cols; c++) heights[c]--;

            switch (isLock)
            {
                case true:
                    locks.Add(heights);
                    break;
                case false:
                    keys.Add(heights);
                    break;
            }
        }
    }

    private bool IsMatch(int[] lockHeights, int[] keyHeights)
    {
        for (int i = 0; i < lockHeights.Length; i++)
        {
            int sum = lockHeights[i] + keyHeights[i];
            if (sum > 5) return false;
        }

        return true;
    }

    public override string Part1() =>
        (from lockHeights in locks from keyHeights in keys where IsMatch(lockHeights, keyHeights) select lockHeights)
        .LongCount().ToString();

    public override string Part2()
    {
        return "";
    }
}