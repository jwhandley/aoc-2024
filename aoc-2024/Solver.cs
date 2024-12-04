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
            Console.WriteLine($"Day {instance.GetNumber()} Part 1: {instance.Part1()} ({watch.Elapsed.Format()})");
            watch.Restart();
            Console.WriteLine($"Day {instance.GetNumber()} Part 2: {instance.Part2()} ({watch.Elapsed.Format()})");
        }
    }
    
    private static string Format(this TimeSpan timeSpan)
    {
        long totalTicks = timeSpan.Ticks;

        switch (totalTicks)
        {
            case >= TimeSpan.TicksPerSecond:
            {
                double seconds = totalTicks / (double)TimeSpan.TicksPerSecond;
                return $"{seconds:F2} s";
            }
            case >= TimeSpan.TicksPerMillisecond:
            {
                double milliseconds = totalTicks / (double)TimeSpan.TicksPerMillisecond;
                return $"{milliseconds:F2} ms";
            }
            case >= 10:
            {
                double microseconds = totalTicks / 10.0;
                return $"{microseconds:F2} µs";
            }
            default:
            {
                double nanoseconds = totalTicks * 100;
                return $"{nanoseconds:F2} ns";
            }
        }
    }
}