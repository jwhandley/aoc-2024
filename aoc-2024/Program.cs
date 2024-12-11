using aoc_2024;

Console.WriteLine("Running all tests");
var tester = new Solver(args[0]);
tester.SolveAll();

Console.WriteLine();
Console.WriteLine("Running all solutions");
var solver = new Solver(args[1]);
solver.SolveAll();
