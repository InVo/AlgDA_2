using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace pa_week_4_1.Tests
{
    [TestClass()]
    public class HeapTests
    {
        [TestMethod()]
        public void AddTest()
        {
            var heap = new Heap<long>(7);
            heap.Add(10);
            List<long> refResult = new List<long> { 10 };
            Assert.IsTrue(refResult.SequenceEqual(heap.Elements));
            heap.Add(2);
            heap.Add(4);
            heap.Add(9);
            heap.Add(11);
            heap.Add(1);
            heap.Add(6);
            refResult = new List<long> { 1, 9, 2, 10, 11, 4, 6 };
            Assert.IsTrue(refResult.SequenceEqual(heap.Elements));
        }

        [TestMethod()]
        public void PopMinTest()
        {
            var heap = new Heap<long>(7);
            heap.Add(10);
            heap.Add(2);
            heap.Add(4);
            heap.Add(9);
            heap.Add(11);
            heap.Add(1);
            heap.Add(6);
            long pop = heap.PopMin();
            Assert.AreEqual(pop, 1);
            List<long> refResult = new List<long> { 2, 9, 4, 10, 11, 6};
            Assert.IsTrue(refResult.SequenceEqual(heap.Elements));
            pop = heap.PopMin();
            Assert.AreEqual(pop, 2);
        }

        [TestMethod()]
        public void RemoveTest()
        {
            var heap = new Heap<long>(7);
            heap.Add(10);
            heap.Add(2);
            heap.Add(4);
            heap.Add(9);
            heap.Add(11);
            heap.Add(1);
            heap.Add(6);
            heap.Remove(2);
            List<long> refResult = new List<long> { 1, 9, 4, 10, 11, 6 };
            Assert.IsTrue(refResult.SequenceEqual(heap.Elements));
            heap.Remove(9);
            refResult = new List<long> { 1, 6, 4, 10, 11 };
            Assert.IsTrue(refResult.SequenceEqual(heap.Elements));
        }
    }
}