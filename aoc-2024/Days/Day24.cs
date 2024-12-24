using System.Text;
using System.Text.RegularExpressions;

namespace aoc_2024.Days;

public class Day24 : BaseDay
{
    private readonly Adder adder = new Adder();


    public Day24()
    {
        string[] parts = Input.Split("\n\n");

        Dictionary<string, IElectric> wires = [];


        foreach (string line in parts[0].Split('\n'))
        {
            string[] tokens = line.Split(": ");
            string name = tokens[0];
            bool value = int.Parse(tokens[1]) > 0;

            var input = new Switch(name, value);
            if (name.StartsWith('x'))
            {
                adder.AddX(input);
            }
            else
            {
                adder.AddY(input);
            }


            wires.Add(name, input);
        }
        
        
        foreach (string line in parts[1].Split('\n'))
        {
            string operation = Regex.Match(line, "OR|XOR|AND").Value;
            string[] tokens = line.Split(" ");
            string a = tokens[0];
            string b = tokens[2];
            string target = tokens[4];

            var wireA = wires.GetValueOrDefault(a, new Wire(a));
            var wireB = wires.GetValueOrDefault(b, new Wire(b));
            var output = wires.GetValueOrDefault(target, new Wire(target)) as Wire;
            var op = operation switch
            {
                "OR" => Operation.Or,
                "XOR" => Operation.Xor,
                "AND" => Operation.And,
                _ => throw new ArgumentOutOfRangeException()
            };

            wires.TryAdd(a, wireA);
            wires.TryAdd(b, wireB);
            wires.TryAdd(target, output!);

            var gate = new Gate(wireA, wireB, op, output!);
            adder.AddGate(gate);

            if (target.StartsWith('z'))
            {
                adder.AddOutput(output!);
            }
        }
    }

    public override string Part1() => adder.Execute().Item1.ToString();

    public override string Part2() => adder.Execute().Item2;
}

public class Switch(string name, bool value) : IElectric
{
    public string Name { get; } = name;
    public bool On { get; set; } = value;
    public override int GetHashCode() => Name.GetHashCode();

    public override string ToString()
    {
        int v = On ? 1 : 0;
        return $"{Name}: {v}";
    }
}

public class Wire(string name) : IElectric
{
    public string Name { get; } = name;
    public bool On { get; set; }
    public override int GetHashCode() => Name.GetHashCode();

    public override string ToString()
    {
        int v = On ? 1 : 0;
        return $"{Name}: {v}";
    }
}

public interface IElectric
{
    public bool On { get; }
    public string Name { get; }
}

public enum Operation
{
    And,
    Or,
    Xor
}

public class Gate(IElectric input1, IElectric input2, Operation operation, Wire target)
{
    public IElectric Input1 { get; } = input1;
    public IElectric Input2 { get; } = input2;
    public Operation Op { get; } = operation;
    public Wire Output { get; set; } = target;
    
    public override string ToString() => $"{Input1} {Op} {Input2} => {Output}";
    
    public bool XorY => (Input1.Name.StartsWith('x') || Input1.Name.StartsWith('y')) && (Input2.Name.StartsWith('x') || Input2.Name.StartsWith('y'));
    public bool IsXor => Op == Operation.Xor;
    public bool HasZOutput => Output.Name.StartsWith('z');
    
    public bool IsStart => Input1.Name is ("x00" or "y00") && Input2.Name is ("x00" or "y00");
}

public class Adder
{
    private List<Gate> Gates { get; } = [];
    private List<Switch> X { get; } = [];
    private List<Switch> Y { get; } = [];
    private List<Wire> Z { get; } = [];

    public void AddGate(Gate gate) => Gates.Add(gate);
    public void AddX(Switch input) => X.Add(input);
    public void AddY(Switch input) => Y.Add(input);
    public void AddOutput(Wire output) => Z.Add(output);

    public void SetX(int value)
    {
        foreach (var s in X) s.On = false;

        foreach (var t in X)
        {
            t.On = value % 2 == 1;
            value >>= 1;
            if (value == 0) break;
        }
    }

    public long GetXValue()
    {
        long result = 0;
        for (int i = 0; i < X.Count; i++)
        {
            if (X[i].On) result |= (1L << i);
        }

        return result;
    }

    public long GetYValue()
    {
        long result = 0;
        for (int i = 0; i < Y.Count; i++)
        {
            if (Y[i].On) result |= (1L << i);
        }

        return result;
    }

    public void SetY(int value)
    {
        foreach (var s in Y) s.On = false;

        foreach (var t in Y)
        {
            t.On = value % 2 == 1;
            value >>= 1;
            if (value == 0) break;
        }
    }

    private IEnumerable<Gate> Neighbors(IElectric input)
    {
        foreach (var gate in Gates)
        {
            if (gate.Input1 == input) yield return gate;
            if (gate.Input2 == input) yield return gate;
        }
    }

    public (long, string) Execute()
    {
        HashSet<IElectric> computed = [..X, ..Y];
        HashSet<string> suspects = [];
        var q = new Queue<Gate>(Gates);

        while (q.Count > 0)
        {
            var gate = q.Dequeue();
            if (computed.Contains(gate.Input1) && computed.Contains(gate.Input2))
            {
                gate.Output.On = gate.Op switch
                {
                    Operation.And => gate.Input1.On & gate.Input2.On,
                    Operation.Or => gate.Input1.On | gate.Input2.On,
                    Operation.Xor => gate.Input1.On ^ gate.Input2.On,
                    _ => throw new ArgumentOutOfRangeException()
                };
                
                if (gate is {IsXor: false, HasZOutput: true} && gate.Output.Name != "z45") suspects.Add(gate.Output.Name);
                if (gate is {IsXor: true, XorY: false} && !gate.Output.Name.StartsWith('z')) suspects.Add(gate.Output.Name);
                if (gate is {IsXor: true} && !Neighbors(gate.Output).All(n => n.Op is Operation.Xor or Operation.And)) suspects.Add(gate.Output.Name);
                if (gate is { Op: Operation.And, IsStart: false } && Neighbors(gate.Output).All(n => n.Op != Operation.Or)) suspects.Add(gate.Output.Name);
                
                computed.Add(gate.Output);
            }
            else
            {
                q.Enqueue(gate);
            }
        }

        List<string> suspectList = suspects.ToList();
        suspectList.Sort();
        
        var sb = new StringBuilder();
        foreach (var w in Z.OrderByDescending(w => w.Name))
        {
            sb.Append(w.On ? '1' : '0');
        }

        string result = sb.ToString();
        return (Convert.ToInt64(result, 2), string.Join(",", suspectList));
    }
}