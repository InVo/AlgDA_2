using Microsoft.VisualStudio.TestTools.UnitTesting;
using pa_week_4_1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pa_week_4_1.Tests
{
    [TestClass()]
    public class ProgramTests
    {
        [TestMethod()]
        public void DijkstraTest()
        {
            Graph g = Program.parseGraph("dijkstra_test.txt");
            long[] results = Program.Dijkstra(g, 0);
            long[] refResults = { 0, 7, 9, 20, 20, 11 };
            Assert.IsTrue(results.SequenceEqual(refResults));
        }

        [TestMethod()]
        public void JacksonTest()
        {
            Graph g = Program.parseGraph("jackson_test.txt");
            bool hnc = false;
            var weights = Program.Jackson(g, out hnc);
            long[] results = new long[weights.Count];
            long[] refResults = { 0, -2, -3, -1, -6, 0 };
            for (int i = 0; i < weights.Count; ++i)
            {
                results[i] = weights[i].Weight;
            }
            Assert.IsTrue(results.SequenceEqual(refResults));
        }

        [TestMethod()]
        public void BellmanFordTest()
        {
            Graph g = Program.parseGraph("dijkstra_test.txt");
            bool hnc = false;
            long[] results = Program.BellmanFord(g, 0, out hnc);
            long[] refResults = { 0, 7, 9, 20, 20, 11 };
            Assert.IsTrue(results.SequenceEqual(refResults));
        }
    }
}