using System.Collections.Generic;
using System.Diagnostics;

namespace pa_week_2_1
{
    class Vertice
    {
        public List<Edge> Edges;

        public Vertice()
        {
            Edges = new List<Edge>();
        }

        public void AddEdge(Edge edge)
        {
            Edges.Add(edge);
        }
    }

    class Edge
    {
        public int[] VerticesIndexes;
        public int Value;

        public Edge(int v1, int v2, int value)
        {
            VerticesIndexes = new int[]{ v1, v2 };
            Value = value;
        }
    }

    class Program
    {
        static Vertice[] vertices;
        static Edge[] edges;
        static int[] clusters;

        const int DESIRED_NUMBER_OF_CLUSTERS = 4;
        static int numberOfClusters;

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines("clustering1.txt");

            int count = int.Parse(lines[0]);
            vertices = new Vertice[count];
            clusters = new int[count];
            edges = new Edge[count * (count - 1) / 2];

            for(int i = 0; i < count; ++i)
            {
                vertices[i] = new Vertice();
                clusters[i] = i;
            }

            numberOfClusters = count;

            for(int i = 1; i < lines.Length; ++i)
            {
                string[] arguments = lines[i].Split(' ');

                int v1 = int.Parse(arguments[0]);
                int v2 = int.Parse(arguments[1]);
                int value = int.Parse(arguments[2]);

                Edge edge = new Edge(v1 - 1, v2 - 1, value);
                vertices[v1 - 1].AddEdge(edge);
                vertices[v2 - 1].AddEdge(edge);
                edges[i - 1] = edge;
            }

            for(int i = 0; i < vertices.Length; ++i)
            {
                Debug.Assert(vertices[i].Edges.Count == count - 1, string.Format("vertice at index {0} should have {1} edges, but have {2}",
                    i, count - 1, vertices[i].Edges.Count));
            }
            
            while(numberOfClusters > DESIRED_NUMBER_OF_CLUSTERS)
            {
                var e = getCurrentMinSeparatedEdge();
                Union(e.VerticesIndexes[0], e.VerticesIndexes[1]);
            }
            var resultEdge = getCurrentMinSeparatedEdge();
            if (resultEdge != null)
            {
                System.Console.WriteLine(string.Format("Result = {0}", resultEdge.Value)); //Correct result: 106
            }
        }

        // Slow union implementation
        static void Union(int v1, int v2)
        {
            int v1Cluster = clusters[v1];
            int v2Cluster = clusters[v2];
            for(int i = 0; i < clusters.Length; ++i)
            {
                if (clusters[i] == v1Cluster)
                {
                    clusters[i] = clusters[v2];
                }
            }
            --numberOfClusters;
        }

        static Edge getCurrentMinSeparatedEdge()
        {
            int minLength = int.MaxValue;
            Edge result = null;
            foreach(var edge in edges)
            {
                // Using fast find implementation
                if (clusters[edge.VerticesIndexes[0]] != clusters[edge.VerticesIndexes[1]]) {
                    if (edge.Value <= minLength)
                    {
                        result = edge;
                        minLength = edge.Value;
                    }
                }
            }
            return result;
        }
    }
}
