using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pa_week_4_1
{
    class Graph
    {
        public List<Vertice> Vertices;
        public List<Edge> Edges;

        public Graph(int verticesCount, int edgesCount)
        {
            Vertices = new List<Vertice>(verticesCount);
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

        public Edge(Vertice tail, Vertice head)
        {
            Tail = tail;
            Head = head;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines("g1.txt");
            string[] firstLineParams = lines[0].Split(' ');

            int verticesCount = int.Parse(firstLineParams[0]);
            int edgesCount = int.Parse(firstLineParams[1]);


        }
    }
}
