namespace aoc_2024.Days;

public class Day18 : BaseDay
{
    private List<(int x, int y)> barrierList;

    private (int dx, int dy)[] directions =
    [
        (-1, 0),
        (1, 0),
        (0, -1),
        (0, 1)
    ];

    public Day18()
    {
        barrierList = [];
        foreach (var line in Input.Split(Environment.NewLine))
        {
            int[] coords = line.Split(',').Select(int.Parse).ToArray();
            barrierList.Add((coords[0], coords[1]));
        }
    }

    public override long Part1()
    {
        return BreadthFirstSearch(0, 0, 70, 70, barrierList.Take(1024).ToHashSet());
    }

    private int BreadthFirstSearch(int x, int y, int tx, int ty, HashSet<(int x, int y)> barriers)
    {
        Queue<(int x, int y, int d)> queue = [];
        HashSet<(int x, int y)> visited = [];
        queue.Enqueue((x, y, 0));

        while (queue.Count > 0)
        {
            (int cx, int cy, int d) = queue.Dequeue();
            if (cx < 0 || cx > tx || cy < 0 || cy > ty || barriers.Contains((cx, cy))) continue;
            if (!visited.Add((cx, cy))) continue;
            if (cx == tx && cy == ty) return d;

            foreach ((int dx, int dy) in directions)
            {
                queue.Enqueue((cx+dx,cy+dy, d+1));
            }
        }

        return -1;
    }

    public override long Part2()
    {
        int left = 0;
        int right = barrierList.Count-1;

        while (left < right)
        {
            int mid = (left + right) / 2;
            HashSet<(int x, int y)> visited = barrierList.Take(mid+1).ToHashSet();

            if (BreadthFirstSearch(0, 0, 70, 70, visited) != -1)
            {
                left = mid + 1;
            }
            else
            {
                right = mid;
            }
        }
        
        Console.WriteLine(barrierList[left]);
        return left;
    }
}