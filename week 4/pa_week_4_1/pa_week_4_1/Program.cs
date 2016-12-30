using System;
using System.CodeDom;
using System.Collections.Generic;


namespace pa_week_4_1
{
    class Graph
    {
        public List<Vertice> Vertices;
        public List<Edge> Edges;

        public Graph(int verticesCount, int edgesCount)
        {
            Vertices = new List<Vertice>(verticesCount);
            for (var i = 1; i <= verticesCount; ++i)
            {
                Vertices.Add(new Vertice {Number = i});
            }


            Edges = new List<Edge>(edgesCount);
        }
    }

    class Vertice
    {
        public int Number;
        public List<Edge> ExitingEdges;
        public List<Edge> IncomingEdges;

        public Vertice()
        {
            ExitingEdges = new List<Edge>();
            IncomingEdges = new List<Edge>();
        }
    }

    class Edge
    {
        public Vertice Tail;
        public Vertice Head;
        public int Weight;

        public Edge(Vertice tail, Vertice head, int weight)
        {
            Tail = tail;
            Head = head;
            Weight = weight;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // All graphs have same size
            // All graphs have negative edges 

            // At current point: g1 and g2 have negative cycles, g3 doesn't have
            bool g1nc = false;
            bool g2nc = false;
            bool g3nc = false;
            var g1 = parseGraph("g1.txt");
            var a = BellmanFord(g1, 1, out g1nc);
            var g2 = parseGraph("g2.txt");
            var b = BellmanFord(g2, 1, out g2nc);
            var g3 = parseGraph("g3.txt");
            var c = BellmanFord(g3, 1, out g3nc);
        }

        static Graph parseGraph(String fileName)
        {
            var lines = System.IO.File.ReadAllLines(fileName);
            var firstLineParams = lines[0].Split(' ');

            var verticesCount = int.Parse(firstLineParams[0]);
            var edgesCount = int.Parse(firstLineParams[1]);

            var graph = new Graph(verticesCount, edgesCount);

            for (var i = 1; i < lines.Length; ++i)
            {
                var arguments = lines[i].Split(' ');
                var tail = int.Parse(arguments[0]);
                var head = int.Parse(arguments[1]);
                var weight = int.Parse(arguments[2]);
                var edge = new Edge(graph.Vertices[tail - 1], graph.Vertices[head - 1], weight);
                graph.Vertices[tail - 1].ExitingEdges.Add(edge);
                graph.Vertices[head - 1].IncomingEdges.Add(edge);
                graph.Edges.Add(edge);
            }
            return graph;
        }

        /// <summary>
        /// Dynamic programming algorithm to compute signle-sourse shortest paths for all other vertices.
        /// Uses 2d array iterating by current vertice number and number of vertices allowed in current path.
        /// A[i, v] = min ( A[i-1, v];  min(A[i-1, w] + C(w,v)  ) ), where C(w,v) = edge cost between vertices w, v.
        /// Base case A[0, v] = 0 if v == s and A[0, v] = long.MaxValue if v != s.
        /// Result is an array with i == n - 1, where n is a number of vertices.
        /// To detect negative cycle, run the algorithm with i == n. If there's no negative cycle, A[n, v] == A[n - 1, v] for every v.
        /// </summary>
        /// <param name="g">Graph</param>
        /// <param name="s">initial vertice index</param>
        static long[] BellmanFord(Graph g, int s, out bool haveNegativeCycles)
        {
            haveNegativeCycles = false;
            int n = g.Vertices.Count;
            long[ , ] A = new long[n + 1, n];
            for(int i = 0; i < n; ++i)
            {
                A[0, i] = int.MaxValue;
            }
            A[0, s - 1] = 0;

            for(int i = 1; i <= n; ++i)
            {
                for (int v = 0; v < n; ++v)
                {
                    long minValue = int.MaxValue;
                    foreach(var edge in g.Vertices[v].IncomingEdges) {
                        long value = edge.Weight + A[i - 1, edge.Tail.Number - 1];
                        if (minValue > value) {
                            minValue = value;
                        }
                    }
                    A[i, v] = Math.Min(minValue, A[i - 1, v]);
                }
            }
            for(int v = 0; v < n; ++v)
            {
                if (A[n, v] != A[n - 1, v])
                {
                    haveNegativeCycles = true;
                    break;
                }
            }
            var result = new long[n];
            for(int v = 0; v < n; ++v)
            {
                result[v] = A[n - 1, v];
            }
            return result;
        }

        /// <summary>
        /// Dynamic programming algorithm for computing all sourse shortest paths
        /// </summary>
        /// <param name="g"></param>
        /// <param name="haveNegativeCycles"></param>
        /// <returns></returns>
        static long FloydWarshall(Graph g, out bool haveNegativeCycles)
        {
            throw new NotImplementedException();
        }
    }
}
