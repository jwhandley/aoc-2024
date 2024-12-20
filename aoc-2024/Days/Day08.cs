namespace aoc_2024.Days;

public class Day08 : BaseDay
{
    private Dictionary<char, List<(int x, int y)>> Map { get; }
    private int Width { get; }
    private int Height { get; }

    public Day08()
    {
        Map = [];
        Height = Input.Split(Environment.NewLine).Length;
        Width = Input.Split(Environment.NewLine)[0].Length;

        foreach ((int r, string line) in Input.Split(Environment.NewLine).Index())
        {
            foreach ((int c, char ch) in line.Index())
            {
                if (ch == '.') continue;
                if (!Map.TryGetValue(ch, out List<(int x, int y)>? list))
                {
                    list = [];
                    Map[ch] = list;
                }

                list.Add((c, r));
            }
        }
    }

    public override string Part1()
    {
        HashSet<(int x, int y)> visited = [];
        foreach (List<(int x, int y)> list in Map.Values)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    (int x1, int y1) = list[i];
                    (int x2, int y2) = list[j];

                    int dx = x2 - x1, dy = y2 - y1;
                    visited.Add((x1 - dx, y1 - dy));
                    visited.Add((x2 + dx, y2 + dy));
                }
            }
        }

        return visited.Count(p => InBounds(p.x, p.y)).ToString();
    }

    private bool InBounds(int x, int y) => 0 <= x && x < Width && 0 <= y && y < Height;

    public override string Part2()
    {
        HashSet<(int x, int y)> visited = [];
        foreach (List<(int x, int y)> list in Map.Values)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count; j++)
                {
                    if (j == i) continue;
                    (int x1, int y1) = list[i];
                    (int x2, int y2) = list[j];

                    int dx = x2 - x1, dy = y2 - y1;

                    int x = x1, y = y1;
                    while (InBounds(x, y))
                    {
                        visited.Add((x, y));
                        x += dx;
                        y += dy;
                    }
                }
            }
        }

        return $"{visited.Count}";
    }
}