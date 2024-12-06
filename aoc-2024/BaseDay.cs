namespace aoc_2024;

public abstract class BaseDay
{
    private const string ClassPrefix = "Day";
    public static string InputDir { get; set; } = "Inputs";
    private string InputFile => Path.Combine(InputDir, $"{GetNumber():D2}.txt");
    protected string Input => File.ReadAllText(InputFile);

    public int GetNumber()
    {
        string typeName = GetType().Name;
        return int.TryParse(typeName[(typeName.IndexOf(ClassPrefix, StringComparison.Ordinal) + ClassPrefix.Length)..], out int i) ? i : default;
    }

    public abstract long Part1();
    public abstract long Part2();
}