namespace aoc_2024.Days;

public class Day17 : BaseDay
{
    private readonly int registerA;
    private readonly int registerB;
    private readonly int registerC;
    private readonly int[] program;

    public Day17()
    {
        string[] parts = Input.Split("\n\n");
        string[] registers = parts[0].Split("\n");
        registerA = int.Parse(registers[0][12..]);
        registerB = int.Parse(registers[1][12..]);
        registerC = int.Parse(registers[2][12..]);

        string programStr = parts[1];
        program = programStr[9..].Split(",").Select(int.Parse).ToArray();
    }
    
    public override string Part1()
    {
        var computer = new TrinaryComputer(registerA, registerB, registerC, program);
        List<long> result = computer.Execute();
        return string.Join(",", result);
    }

    public override string Part2()
    {
        Stack<int> stack = new(program);
        List<long> possibilities = [1,2,3,4,5,6,7];
        
        while (stack.Count > 0)
        {
            List<long> next = [];
            int target = stack.Pop();

            foreach (long value in possibilities)
            {
                var computer = new TrinaryComputer(value, 0, 0, program);
                List<long> result = computer.Execute();

                if (result.First() != target) continue;
                
                if (stack.Count == 0) return value.ToString();
                for (int i = 0; i < 8; i++)
                {
                    next.Add(value * 8 + i);    
                }   
            }
            
            possibilities = next;
        }
        
        return "";
    }
}

public class TrinaryComputer(long registerA, int registerB, int registerC, int[] program)
{
    private long RegisterA { get; set; } = registerA;
    private long RegisterB { get; set; } = registerB;
    private long RegisterC { get; set; } = registerC;
    private int[] Program { get; } = program;
    private int StackPointer { get; set; }

    public List<long> Execute()
    {
        List<long> numbers = [];
        while (StackPointer < Program.Length)
        {
            switch (Program[StackPointer])
            {
                case 0:
                {
                    long numerator = RegisterA;
                    long denominator = (long)Math.Pow(2, ComboOperand(Program[StackPointer + 1]));
                    RegisterA = numerator / denominator;
                    break;
                }
                case 1:
                    RegisterB ^= Program[StackPointer+1];
                    break;
                case 2:
                    RegisterB = ComboOperand(Program[StackPointer + 1]) % 8;
                    break;
                case 3:
                {
                    if (RegisterA != 0)
                    {
                        StackPointer = Program[StackPointer+1];
                        continue;
                    }
                    break;
                }
                case 4:
                    RegisterB ^= RegisterC;
                    break;
                case 5:
                {
                    long output = ComboOperand(Program[StackPointer + 1]) % 8;
                    numbers.Add(output);
                    break;
                }
                case 6:
                {
                    long numerator = RegisterA;
                    long denominator = (long)Math.Pow(2, ComboOperand(Program[StackPointer + 1]));
                    RegisterB = numerator / denominator;
                    break;
                }
                case 7:
                {
                    long numerator = RegisterA;
                    long denominator = (long)Math.Pow(2, ComboOperand(Program[StackPointer + 1]));
                    RegisterC = numerator / denominator;
                    break;
                }
            }

            StackPointer += 2;
        }

        
        return numbers;
    }

    private long ComboOperand(int operand) => operand switch
    {
        <= 3 => operand,
        4 => RegisterA,
        5 => RegisterB,
        6 => RegisterC,
        _ => throw new ArgumentException()
    };
}