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

    public override long Part1()
    {
        HashSet<(int x, int y)> visited = [];
        foreach (KeyValuePair<char, List<(int x, int y)>> e in Map)
        {
            for (int i = 0; i < e.Value.Count; i++)
            {
                for (int j = i + 1; j < e.Value.Count; j++)
                {
                    (int x1, int y1) = e.Value[i];
                    (int x2, int y2) = e.Value[j];
                    
                    int dx = x2 - x1, dy = y2 - y1;

                    if (InBounds(x1 - dx, y1 - dy))
                    {
                        visited.Add((x1-dx, y1-dy));    
                    }

                    if (InBounds(x2 + dx, y2 + dy))
                    {
                        visited.Add((x2+dx, y2+dy));    
                    }
                    
                }
            }
        }
        
        return visited.Count;
    }
    
    private bool InBounds(int x, int y) => 0 <= x && x < Width && 0 <= y && y < Height;

    public override long Part2()
    {
        HashSet<(int x, int y)> visited = [];
        foreach (KeyValuePair<char, List<(int x, int y)>> e in Map)
        {
            for (int i = 0; i < e.Value.Count; i++)
            {
                for (int j = i+1; j < e.Value.Count; j++)
                {
                    (int x1, int y1) = e.Value[i];
                    (int x2, int y2) = e.Value[j];
                    
                    int dx = x2 - x1, dy = y2 - y1;
                    
                    int x = x1, y = y1;
                    while (InBounds(x, y))
                    {
                        visited.Add((x, y));
                        x += dx;
                        y += dy;
                    }

                    x = x1;
                    y = y1;
                    while (InBounds(x, y))
                    {
                        visited.Add((x, y));
                        x -= dx;
                        y -= dy;
                    }
                }
            }
        }
        
        return visited.Count;
    }
}