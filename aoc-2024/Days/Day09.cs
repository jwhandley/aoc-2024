namespace aoc_2024.Days;

public class Day09 : BaseDay
{
    private readonly int[] blocks;
    private readonly List<(int idx, int length)> files;
    private readonly List<(int idx, int length)> emptySpaces;
    private readonly List<int> blanks;

    public Day09()
    {
        blocks = [];
        files = [];
        emptySpaces = [];
        blanks = [];
        
        char[] c = Input.ToCharArray();
        int size = c.Select(v => v - '0').Sum();
        
        blocks = new int[size];
        int idx = 0;
        for (int i = 0; i < c.Length; ++i)
        {
            int v = c[i] - '0';
            if (i % 2 != 0)
            {
                emptySpaces.Add((idx, v));
                for (int j = 0; j < v; ++j)
                {
                    blocks[idx] = -1;
                    blanks.Add(idx);
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

    public override long Part1()
    {
        int[] blocksCopy = [..blocks];
        int right = blocksCopy.Length - 1;
        foreach (int idx in blanks)
        {
            while (blocksCopy[right] == -1) right--;
            if (right <= idx) break;
            blocksCopy[idx] = blocksCopy[right];
            blocksCopy[right] = -1;
            right--;
        }
        
        long result = 0;
        for (int i = 0; i < right+1; i++)
        {
            if (blocksCopy[i] == -1) continue;
            result += blocksCopy[i] * i;
        }

        return result;
    }

    public override long Part2()
    {
        for (int i = files.Count - 1; i >= 0; i--)
        {
            (int idx, int length) = files[i];

            foreach ((int j, (int start, int size)) in emptySpaces.Index())
            {
                if (start >= idx)
                {
                    emptySpaces.RemoveAt(j);
                    break;
                }

                if (size < length) continue;
                files[i] = (start, length);
                if (size == length)
                {
                    emptySpaces.RemoveAt(j);
                }
                else
                {
                    emptySpaces[j] = (start + length, size - length);
                }

                break;
            }
        }

        long result = 0;
        foreach ((int id, (int pos, int length)) in files.Index())
        {
            for (int i = pos; i < pos + length; i++)
            {
                result += id * i;
            }
        }

        return result;
    }

    private int? FirstEmptySpace(int length)
    {
        for (int i = 0; i + length < blocks.Length; ++i)
        {
            if (blocks[i] != -1) continue;

            bool found = true;
            for (int j = 0; j < length; ++j)
            {
                if (blocks[i + j] != -1) found = false;
            }

            if (found) return i;
        }

        return null;
    }
}