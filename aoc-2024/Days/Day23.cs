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

    public override string Part1() => graph.ThreeConnected().Count(c => c.Any(n => n.StartsWith('t'))).ToString();
    public override string Part2()
    {
        List<string> best = graph.ConnectedComponents().MaxBy(c => c.Count)!.ToList();
        best.Sort();
        return string.Join(",", best);
    }
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

    public List<HashSet<string>> ConnectedComponents()
    {
        HashSet<HashSet<string>> components = new(new HashSetValueComparer());

        foreach (string n in AdjacencyList.Keys)
        {
            HashSet<string> component = [n];
            foreach (string n1 in AdjacencyList[n])
            {
                if (!component.All(n2 => n1 != n2 && HasEdge(n1, n2))) continue;
                component.Add(n1);
            }
            components.Add(component);
        }

        return components.ToList();
    }

    public List<HashSet<string>> ThreeConnected()
    {
        HashSet<HashSet<string>> visited = new(new HashSetValueComparer());
        List<HashSet<string>> components = [];

        foreach (string n in AdjacencyList.Keys)
        {
            foreach (string n1 in AdjacencyList[n])
            {
                foreach (string n2 in AdjacencyList[n1])
                {
                    if (n1 == n2) continue;
                    if (!HasEdge(n, n2)) continue;
                    if (!visited.Add([n, n1, n2])) continue;
                    components.Add([n, n1, n2]);
                }
            }
        }

        return components;
    }
}

public class HashSetValueComparer : IEqualityComparer<HashSet<string>>
{
    public bool Equals(HashSet<string>? x, HashSet<string>? y)
    {
        // If both are null, or both are the same reference, they are equal
        if (ReferenceEquals(x, y)) return true;

        // If one is null but not the other, they are not equal
        if (x == null || y == null) return false;

        // Compare contents using SetEquals
        return x.SetEquals(y);
    }

    public int GetHashCode(HashSet<string>? obj)
    {
        return obj == null ? 0 : obj.Aggregate(0, (hash, item) => hash ^ item.GetHashCode());
    }
}