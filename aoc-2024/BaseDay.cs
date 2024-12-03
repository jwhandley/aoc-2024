namespace aoc_2024;

public abstract class BaseDay
{
    private const string ClassPrefix = "Day";
    private const string InputDir = "Inputs";
    
    public static bool UseTestInput { get; set; } = false;
    private string InputFile => UseTestInput ? Path.Combine(InputDir, $"{GetNumber():D2}_test.txt") : Path.Combine(InputDir, $"{GetNumber():D2}.txt");
    protected string Input => File.ReadAllText(InputFile);
    private int GetNumber()
    {
        string typeName = GetType().Name;
        return int.TryParse(typeName[(typeName.IndexOf(ClassPrefix, StringComparison.Ordinal) + ClassPrefix.Length)..], out int i) ? i : default;
    }

    public abstract long Part1();
    public abstract long Part2();
}