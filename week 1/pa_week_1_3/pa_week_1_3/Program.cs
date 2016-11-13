using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace pa_week_1_3
{

    class Heap<T>
    {
        // Returns true if a > b, false if a <= b
        public delegate bool Compare(T a, T b);
        public Compare CompareFunction;

        List<T> _values;
        bool _isMin = true;

        public int Count { get { return _values.Count; } }

        public Heap()
        {
            _values = new List<T>();
        }

        public Heap(int count)
        {
            _values = new List<T>(count);
        }

        public Heap(T[] values): this(values.Length)
        {
            foreach(T value in values)
            {
                Add(value);
            }
        }

        public void Add(T value)
        {
            _values.Add(value);
            ValidateOnAdd();
        }

        public T Remove()
        {
            if (_values.Count > 0)
            {
                T result = _values[0];
                _values[0] = _values[_values.Count - 1];
                _values.RemoveAt(_values.Count - 1);
                ValidateOnRemove();
                return result;
            } else
            {
                return default(T);
            }
        }

        public bool IsEmpty()
        {
            return _values.Count == 0;
        }

        /// <summary>
        /// Validates heap order assuming that last value has been added
        /// and other values already keep heap order
        /// </summary>
        protected void ValidateOnAdd()
        {
            bool change = true;
            int index = _values.Count - 1;
            while (change && index > 0)
            {
                change = false;
                int parentIndex = (index - 1) / 2;
                T currentValue = _values[index];
                T parentValue = _values[parentIndex];

                // Swap if currentValue > parentValue (maxHeap)
                // or currentValue <= parentValue (minHeap)
                if (!CompareFunction(currentValue, parentValue) && _isMin)
                {
                    _values[parentIndex] = currentValue;
                    _values[index] = parentValue;
                    index = parentIndex;
                    change = true;
                }
            }
        }

        protected void ValidateOnRemove()
        {
            bool change = true;
            int index = 0;
            while(change && index < _values.Count)
            {
                change = false;

                T currentValue = _values[index];
                bool haveLeftChild = _values.Count > 2 * index + 1;
                bool haveRightChild = _values.Count > 2 * index + 2;
                T leftChildValue = haveLeftChild ? _values[2 * index + 1] : default(T);
                T rightChildValue = haveRightChild ? _values[2 * index + 2] : default(T);

                int indexForChange = -1;
                if (haveLeftChild && !CompareFunction(currentValue, leftChildValue) && _isMin)
                {
                    indexForChange = 2 * index + 1;
                    change = true;
                }
                else if (haveRightChild && !CompareFunction(currentValue, rightChildValue) && _isMin) {
                    indexForChange = 2 * index + 2;
                    change = true;
                }

                if (change)
                {
                    _values[index] = _values[indexForChange];
                    _values[indexForChange] = currentValue;
                    index = indexForChange;
                }
            }
        }
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

        public void AddEdge(long vertice1, long vertice2, long value)
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
        public long Weight;
        public List<GraphEdge> Edges;
        public bool IsInMST;

        public GraphVertice()
        {
            Edges = new List<GraphEdge>();
            Weight = long.MaxValue;
            IsInMST = false;
    }

        public void AddEdge(GraphEdge edge)
        {
            Edges.Add(edge);
        }
    }

    class GraphEdge
    {
        public GraphVertice[] Vertices;
        public long Value { get; private set; }

        public GraphEdge(GraphVertice start, GraphVertice end, long value)
        {
            Vertices = new GraphVertice[] { start, end };
            Value = value;
        }
    }

    class Program
    {
        static List<GraphVertice> mstVertices = new List<GraphVertice>();
        static Heap<GraphVertice> heap;

        static long mstValue = 0;
        
        static bool Compare(GraphVertice a, GraphVertice b)
        {
            return a.Weight >= b.Weight;
        }

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
            heap = new Heap<GraphVertice>(vertCount);
            heap.CompareFunction = Compare;

            for (int i = 1; i < lines.Length; ++i)
            {
                string[] arguments = lines[i].Split(' ');

                Debug.Assert(arguments.Length == 3, string.Format("Number of arguments in line {0} should be 3", i));

                long vertice1 = long.Parse(arguments[0]);
                long vertice2 = long.Parse(arguments[1]);
                long value = long.Parse(arguments[2]);
                graph.AddEdge(vertice1, vertice2, value);
            }

            Debug.Assert(graph.Vertices.Length == vertCount, "Vertices count should be equal to the value in file");
            Debug.Assert(graph.Edges.Length == edgeCount, "Vertices count should be equal to the value in file");


            graph.Vertices[0].Weight = 0;
            AddVerticeToMST(graph.Vertices[0]);
            //heap = new Heap<GraphVertice>(graph.Vertices);

            while (heap.Count > 0)
            {
                var vertice = heap.Remove();
                AddVerticeToMST(vertice);
            }

            Console.WriteLine(string.Format("Result = {0}", mstValue)); // Correct Result -3612829
        }

        static void AddVerticeToMST(GraphVertice vertice)
        {
            vertice.IsInMST = true;
            mstVertices.Add(vertice);
            mstValue += vertice.Weight;
            vertice.IsInMST = true;

            foreach(var edge in vertice.Edges)
            {
                var vert = Array.Find(edge.Vertices, v => !v.IsInMST);
                if (vert != null)
                {
                    if (vert.Weight > edge.Value)
                    {
                        vert.Weight = edge.Value;
                    }
                    //TODO: need to optimise heap validation at this moment
                    heap.Add(vert);
                }
            }
        }
    }
}
