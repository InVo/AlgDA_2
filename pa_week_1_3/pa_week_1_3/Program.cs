using System.Collections.Generic;
using System.Diagnostics;

namespace pa_week_1_3
{

    class Heap
    {

    }

    class Graph
    {
        public GraphVertice[] Vertices;
        public GraphEdge[] Edges;

        private int _edgesCount = 0;

        public Graph(int verticesNum, int edgesNum)
        {
            Vertices = new GraphVertice[verticesNum];
            for(int i = 0; i < verticesNum; ++i)
            {
                Vertices[i] = new GraphVertice();
            }

            Edges = new GraphEdge[edgesNum];
        }

        public void AddEdge(int vertice1, int vertice2, int value)
        {
            var vert1 = Vertices[vertice1 - 1];
            var vert2 = Vertices[vertice2 - 1];
            GraphEdge edge = new GraphEdge(vert1, vert2, value);
            vert1.AddEdge(edge);
            vert2.AddEdge(edge);
            Edges[_edgesCount] = edge;
            ++_edgesCount;
        }
    }

    class GraphVertice
    {
        private List<GraphEdge> _edges;

        public GraphVertice()
        {
            _edges = new List<GraphEdge>();
        }

        public void AddEdge(GraphEdge edge)
        {
            _edges.Add(edge);
        }
    }

    class GraphEdge
    {
        private GraphVertice[] _endPoints;
        public int Value { get; private set; }

        public GraphEdge(GraphVertice start, GraphVertice end, int value)
        {
            _endPoints = new GraphVertice[] { start, end };
            Value = value;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines("edges.txt");

            if (lines.Length == 0)
            {
                return;
            }

            string[] counts = lines[0].Split(' ');
            Debug.Assert(counts.Length == 2, "File first line should be 2 (vertices and edges count)");

            int vertCount = int.Parse(counts[0]);
            int edgeCount = int.Parse(counts[1]);

            var graph = new Graph(vertCount, edgeCount);

            for (int i = 1; i < lines.Length; ++i)
            {
                string[] arguments = lines[i].Split(' ');

                Debug.Assert(arguments.Length == 3, string.Format("Number of arguments in line {0} should be 3", i));

                int vertice1 = int.Parse(arguments[0]);
                int vertice2 = int.Parse(arguments[1]);
                int value = int.Parse(arguments[2]);
                graph.AddEdge(vertice1, vertice2, value);
            }

            Debug.Assert(graph.Vertices.Length == vertCount, "Vertices count should be equal to the value in file");
            Debug.Assert(graph.Edges.Length == edgeCount, "Vertices count should be equal to the value in file");


        }
    }
}
