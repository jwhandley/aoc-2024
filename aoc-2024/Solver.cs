using System.Diagnostics;

namespace aoc_2024;

public static class Solver
{
    public static void SolveAll()
    {
        BaseDay.UseTestInput = false;
        RunAll();
    }
    
    public static void TestAll()
    {
        BaseDay.UseTestInput = true;
        RunAll();
    }

    private static void RunAll()
    {
        var dayType = typeof(BaseDay);
    
        IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(t => dayType.IsAssignableFrom(t) && t != dayType);
            
        var watch = Stopwatch.StartNew();
        foreach (var type in types)
        {
            var instance = (BaseDay)Activator.CreateInstance(type)!;
            watch.Restart();
            Console.WriteLine($"{instance.GetType().Name} Part 1: {instance.Part1()} ({watch.ElapsedMilliseconds}ms)");
            watch.Restart();
            Console.WriteLine($"{instance.GetType().Name} Part 2: {instance.Part2()} ({watch.ElapsedMilliseconds}ms)");
        }
    }
}