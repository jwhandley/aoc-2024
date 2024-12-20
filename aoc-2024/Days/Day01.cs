namespace aoc_2024.Days;

public class Day01 : BaseDay
{
    public override string Part1()
    {
        var left = new List<int>();
        var right = new List<int>();

        foreach (string line in Input.Split('\n'))
        {
            string[] numbers = line.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
            left.Add(int.Parse(numbers[0]));
            right.Add(int.Parse(numbers[1]));
        }
        
        left.Sort();
        right.Sort();

        long result = 0;
        for (int i = 0; i < left.Count; ++i)
        {
            result += Math.Abs(left[i] - right[i]);
        }
        
        return $"{result}";
    }

    public override string Part2()
    {
        var list = new List<int>();
        var counts = new Dictionary<int, int>();

        foreach (string line in Input.Split('\n'))
        {
            string[] numbers = line.Split(Array.Empty<char>(), StringSplitOptions.RemoveEmptyEntries);
            int left = int.Parse(numbers[0]);
            int right = int.Parse(numbers[1]);
            
            list.Add(left);
            counts[right] = counts.GetValueOrDefault(right) + 1;
        }

        long result = list.Aggregate<int, long>(0, (current, item) => current + item * counts.GetValueOrDefault(item));
        return $"{result}";
    }
}