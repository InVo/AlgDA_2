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

        public Vertice()
        {
            ExitingEdges = new List<Edge>();
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
            var g1 = parseGraph("g1.txt");
            var g2 = parseGraph("g2.txt");
            var g3 = parseGraph("g3.txt");
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
                graph.Edges.Add(edge);
            }
            return graph;
        }
    }
}
