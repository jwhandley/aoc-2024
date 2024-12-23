namespace aoc_2024.Days;

public class Day23 : BaseDay
{
    private readonly Graph graph;

    public Day23()
    {
        graph = new Graph();
        foreach (string line in Input.Split(Environment.NewLine))
        {
            string[] nodes = line.Split('-');
            graph.AddEdge(nodes[0], nodes[1]);
        }
    }

    public override string Part1() => $"{graph.ThreeConnected().Count(c => c.Any(n => n.StartsWith('t'))) / 6}";
    public override string Part2() => string.Join(",", graph.ConnectedComponents().MaxBy(c => c.Count) ?? []);
}

public class Graph
{
    private Dictionary<string, HashSet<string>> AdjacencyList { get; } = [];

    public void AddEdge(string node1, string node2)
    {
        if (!AdjacencyList.TryGetValue(node1, out HashSet<string>? node1Neighbors))
        {
            node1Neighbors = [];
        }

        node1Neighbors.Add(node2);
        AdjacencyList[node1] = node1Neighbors;

        if (!AdjacencyList.TryGetValue(node2, out HashSet<string>? node2Neighbors))
        {
            node2Neighbors = [];
        }

        node2Neighbors.Add(node1);
        AdjacencyList[node2] = node2Neighbors;
    }

    private bool HasEdge(string node1, string node2) => AdjacencyList[node1].Contains(node2);

    public IEnumerable<List<string>> ConnectedComponents() => AdjacencyList.Keys.Select(n =>
    {
        var component = new HashSet<string> { n };
        foreach (string n1 in AdjacencyList[n].Where(n1 => component.All(n2 => n1 != n2 && HasEdge(n1, n2))))
        {
            component.Add(n1);
        }
        
        List<string> result = component.ToList();
        result.Sort();
        return result;
    });

    public IEnumerable<List<string>> ThreeConnected() => 
        from n in AdjacencyList.Keys
        from n1 in AdjacencyList[n]
        from n2 in AdjacencyList[n1]
        where HasEdge(n, n2)
        select new List<string> { n, n1, n2 };
}