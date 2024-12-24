using aoc_2024;

string baseDirectory = AppContext.BaseDirectory;
// Console.WriteLine("Running all tests");
// var tester = new Solver(Path.Join(baseDirectory,"Tests"));
// tester.SolveAll();

Console.WriteLine();
Console.WriteLine("Running all solutions");
var solver = new Solver(Path.Join(baseDirectory,"Inputs"));
solver.SolveAll();
