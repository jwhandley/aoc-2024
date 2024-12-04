namespace aoc_2024.Days;

public class Day02 : BaseDay
{
    public override long Part1()
    {
        return Input.Split('\n').Count(line => IsSafe(line.Split(' ').Select(int.Parse).ToArray()));
    }

    private static bool IsSafe(int[] report)
    {
        bool increasing = report[1] > report[0];
        for (int i = 1; i < report.Length; i++)
        {
            if (report[i] > report[i - 1] != increasing) return false;

            int diff = Math.Abs(report[i] - report[i - 1]);
            if (diff is < 1 or > 3) return false;
        }

        return true;
    }

    private static bool IsSafeWithRemoval(int[] report)
    {
        for (int ignore = 0; ignore < report.Length; ignore++)
        {
            int[] modifiedReport = new int[report.Length-1];
            int j = 0;
            for (int i = 0; i < report.Length; i++)
            {
                if (i == ignore) continue;
                modifiedReport[j] = report[i];
                j++;
            }
            
            if (IsSafe(modifiedReport)) return true;
        }

        return false;
    }

   
    public override long Part2()
    {
        return Input.Split('\n').Count(line => IsSafeWithRemoval(line.Split(' ').Select(int.Parse).ToArray()));
    }
}