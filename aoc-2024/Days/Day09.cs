namespace aoc_2024.Days;

public class Day09 : BaseDay
{
    private readonly int?[] blocks;
    private readonly List<(int idx, int length)> files;
    private readonly HashSet<(int idx, int length)> emptySpaces;

    public Day09()
    {
        blocks = [];
        files = [];
        emptySpaces = [];
        
        char[] c = Input.ToCharArray();
        int size = c.Select(v => v - '0').Sum();
        
        blocks = new int?[size];
        int idx = 0;
        for (int i = 0; i < c.Length; ++i)
        {
            int v = c[i] - '0';
            if (i % 2 != 0)
            {
                emptySpaces.Add((idx, v));
                for (int j = 0; j < v; ++j)
                {
                    blocks[idx] = null;
                    idx++;
                }
            }
            else
            {
                files.Add((idx, v));
                for (int j = 0; j < v; ++j)
                {
                    blocks[idx] = i/2;
                    idx++;
                }
            }
        }
    }

    public override string Part1()
    {
        int head = 0;
        int tail = blocks.Length - 1;
        while (true)
        {
            while (blocks[head] != null) head++;
            while (blocks[tail] == null) tail--;

            if (head >= tail) break;
            
            blocks[head++] = blocks[tail];
            blocks[tail--] = null;
        }
        
        return blocks.Select((v, i) => v == null ? 0L : (long)v * i).Sum().ToString();
    }

    public override string Part2()
    {
        for (int i = files.Count - 1; i >= 0; i--)
        {
            (int idx, int length) = files[i];

            (int start, int size) = emptySpaces.FirstOrDefault(e=> e.length >= length && e.idx < idx);
            if (size == 0) continue;
            files[i] = (start, length);
            emptySpaces.Remove((start, size));
            if (size != length)
            {
                emptySpaces.Add((start + length, size - length));
            }
        }

        
        long result = 0;
        foreach ((int id, (int pos, int length)) in files.Index())
        {
            long tmp = 0;
            for (int i = pos; i < pos + length; i++)
            {
                tmp += id * i;
            }
            result += tmp;
        }
        
        return result.ToString();
    }
}

