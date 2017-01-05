using System;
using System.Collections.Generic;


namespace pa_week_4_1
{
    //Required for Dijkstra's fast implementation
    public class Heap<T> where T : IComparable<T> {
		List<T> _elements;

        public List<T> Elements { get { return _elements; } }
		
		public int Count { get { return _elements.Count; } }
		
		public Heap(int size) {
			_elements = new List<T>();
		}
		
		public void Add(T elem) {
			_elements.Add(elem);
			Verify();
		}
		
		public T PopMin() {
			if (_elements.Count > 0) {
				T result = _elements[0];
				Remove(result);
				return result;				
			}
			return default(T);
		}
		
		public void Remove(T val) {
            int i = _elements.IndexOf(val);
			if (i >= 0) {
				_elements[i] = _elements[_elements.Count - 1];
				_elements.RemoveAt(_elements.Count - 1);
				VerifyOnDeletion(i);
			}
		}
		
		private void VerifyOnDeletion(int index) {
			int root = index;
			int left = 2 * index + 1;
			int right = 2 * index + 2;
			while (left < _elements.Count && _elements[root].CompareTo(_elements[left]) > 0 ||
				   right < _elements.Count && _elements[root].CompareTo(_elements[right]) > 0) {
				if (_elements[root].CompareTo(_elements[left]) > 0) {
					Swap(root, left);
					root = left;
				} 
				else {
					Swap(root, right);
					root = right;
				}
				left = 2 * root + 1;
				right = 2 * root + 2;
			}
		}
		
		private void Verify() {
			int curIndex = _elements.Count - 1;
            int parentIndex = (curIndex - 1) / 2;
			while (curIndex > 0 && 
                   _elements[curIndex].CompareTo(_elements[parentIndex]) < 0) {
				Swap(curIndex, parentIndex);
				curIndex = parentIndex;
                parentIndex = (curIndex - 1) / 2;
			}
		}
		
		private void Swap(int i1, int i2) {
			T temp = _elements[i1];
			_elements[i1] = _elements[i2];
			_elements[i2] = temp;
		}
	}

    public class Graph
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

        /// <summary>
        /// Creates full independent copy of this graph
        /// </summary>
        /// <returns></returns>
        public Graph Copy()
        {
            var newGraph = new Graph(Vertices.Count, Edges.Count);
            for(int i = 0; i < Vertices.Count; ++i)
            {
                newGraph.Vertices[i].Number = Vertices[i].Number;
            }
            foreach(var e in Edges)
            {
                var tail = newGraph.Vertices.Find(v => v.Number == e.Tail.Number);
                var head = newGraph.Vertices.Find(v => v.Number == e.Head.Number);
                var newEdge = new Edge(tail, head, e.Weight);
                tail.ExitingEdges.Add(newEdge);
                head.IncomingEdges.Add(newEdge);
                newGraph.Edges.Add(newEdge);
            }
            return newGraph;
        }
    }

    public class Vertice
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

    public class Edge
    {
        public Vertice Tail;
        public Vertice Head;
        public long Weight;

        public Edge(Vertice tail, Vertice head, long weight)
        {
            Tail = tail;
            Head = head;
            Weight = weight;
        }
    }

    public class Program
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
            var a = ASSPByJohnsonAndDijkstra(g1, out g1nc);
            var g2 = parseGraph("g2.txt");
            var b = ASSPByJohnsonAndDijkstra(g2, out g2nc);
            var g3 = parseGraph("g3.txt");
            var c = ASSPByJohnsonAndDijkstra(g3, out g3nc);
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
        /// Dynamic programming algorithm to compute single-source shortest paths for all other vertices.
        /// Uses 2d array iterating by current vertice number and number of vertices allowed in current path.
        /// A[i, v] = min ( A[i-1, v];  min(A[i-1, w] + C(w,v)  ) ), where C(w,v) = edge cost between vertices w, v.
        /// Base case A[0, v] = 0 if v == s and A[0, v] = long.MaxValue if v != s.
        /// Result is an array with i == n - 1, where n is a number of vertices.
        /// To detect negative cycle, run the algorithm with i == n. If there's no negative cycle, A[n, v] == A[n - 1, v] for every v.
        /// </summary>
        /// <param name="g">Graph</param>
        /// <param name="s">initial vertice index (should be Number - 1)</param>
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
		
		private class VerticeWeight : IComparable<VerticeWeight> {
			public Vertice Vertice;
			public long Weight;
			
			public VerticeWeight(Vertice v, long w) {
				Vertice = v;
				Weight = w;
			}

            public int CompareTo(VerticeWeight other) {
				if (Weight > other.Weight) {
					return 1;
				} 
				else if (Weight == other.Weight) {
					return 0;
				}
				return -1;
			}
        }
		



        /// <summary>
        /// Greedy algorithm to compute single-source shortest paths for all other vertices.
		/// All vertices are put into min heap with value assigned to each vertice which describes
		/// shortest path length from s so far. On each iteration, vertice with min value picked from the heap
		/// and added to passed vertices array. Values are updated for all vertices not in passed vertices 
		/// at the end of all edges incident from picked vertice. Additional vertice pointer for each vertice is required
		/// to restore whole shortest path. O(nlog(n)) time.
		/// WARNING - Algorithm is only correct on graphs with no negative edges!
		/// When using on general graphs, it is required to run Johnson's algorithm first and then recompute edges weights back.
        /// </summary>
        /// <param name="g">Graph</param>
        /// <param name="s">initial vertice index</param>		
		static long[] Dijkstra(Graph g, int s) {
			Heap<VerticeWeight> vertHeap = new Heap<VerticeWeight>(g.Vertices.Count);
			
			// Used for O(1) access to vertice by index
			List<VerticeWeight> verticeWeights = new List<VerticeWeight>(g.Vertices.Count);
			List<Vertice> passedVertices = new List<Vertice>();
			for(int i = 0; i < g.Vertices.Count; ++i) {
				var vw = new VerticeWeight(g.Vertices[i], s == i ? 0 : int.MaxValue);
				vertHeap.Add(vw);
				verticeWeights.Add(vw);
			}
			
			while(vertHeap.Count > 0) {
				VerticeWeight vw = vertHeap.PopMin();
                passedVertices.Add(vw.Vertice);
                foreach (var edge in vw.Vertice.ExitingEdges) {
					int i = edge.Head.Number - 1;
					if (!passedVertices.Contains(vw.Vertice) && 
						verticeWeights[i].Weight > vw.Weight + edge.Weight) {
						verticeWeights[i].Weight = vw.Weight + edge.Weight;
						vertHeap.Remove(vw);
						vertHeap.Add(vw);
					}
				}
			}
			long[] result = new long[g.Vertices.Count];
			for(int i = 0; i < result.Length; ++i) {
				result[i] = verticeWeights[i].Weight;
			}
			return result;
		}




        static List<VerticeWeight> Johnson(Graph g, out bool haveNegativeCycles)
        {
            var newGraph = g.Copy();
            var newVert = new Vertice();
            newVert.Number = g.Vertices.Count + 1;
            foreach (var v in newGraph.Vertices)
            {
                var newEdge = new Edge(newVert, v, 0);
                newVert.ExitingEdges.Add(newEdge);
                newGraph.Edges.Add(newEdge);
            }
            long[] resultWeights = BellmanFord(newGraph, newVert.Number - 1, out haveNegativeCycles);
            var result = new List<VerticeWeight>();
            for(int i = 0; i < resultWeights.Length; ++i)
            {
                result.Add(new VerticeWeight(g.Vertices[i], resultWeights[i]));
            }
            return result;
        }




        static long[ , ] ASSPByJohnsonAndDijkstra(Graph g, out bool haveNegativeCycles)
        {
            var verticeWeights = Johnson(g, out haveNegativeCycles);
            foreach (var e in g.Edges)
            {
                var tail = verticeWeights.Find(vw => vw.Vertice == e.Tail);
                var head = verticeWeights.Find(vw => vw.Vertice == e.Head);
                e.Weight += tail.Weight - head.Weight;
                if (e.Weight < 0 && !haveNegativeCycles)
                {
                    throw new ArithmeticException("After Johnson algorithm edge weight can not be negative");
                }
            }
            long[,] result = new long[g.Vertices.Count, g.Vertices.Count];
            for(int i = 0; i < g.Vertices.Count; ++i)
            {
                var dijRes = Dijkstra(g, i);
                for(int j = 0; j < dijRes.Length; ++ j)
                {
                    var sourceWeight = verticeWeights.Find(vw => vw.Vertice.Number == i + 1);
                    var endWeight = verticeWeights.Find(vw => vw.Vertice.Number == j + 1);
                    result[i, j] = dijRes[j] + endWeight.Weight - sourceWeight.Weight;
                }
            }
            return result;
        }




        /// <summary>
        /// Dynamic programming algorithm for computing all sourse shortest paths.
		/// Uses 3d-array iteration, A[i, j, k] = min path from i to j using only {1,..,k} vertices (not counting i and j)
		/// A[i, j, k] = need to remember...
		/// Resulting values are stored in A[i, j, n]. O(m * n^2) time.
        /// </summary>
        /// <param name="g"></param>
        /// <param name="haveNegativeCycles"></param>
        /// <returns></returns>
        static long[ , ] FloydWarshall(Graph g, out bool haveNegativeCycles)
        {
            throw new NotImplementedException();
        }
    }
}
