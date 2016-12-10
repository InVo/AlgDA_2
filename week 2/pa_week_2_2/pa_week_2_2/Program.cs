using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pa_week_2_2
{
    class Element
    {
        public bool[] Bits;
        public int OnesCount = 0;
        public long SortingValue = 0;
        public int Index = 0;

        public Element(int size)
        {
            Bits = new bool[size];
        }

        public void SetBit(int index, bool value)
        {
            Bits[index] = value;
        }

        public override string ToString()
        {
            string result = "";
            foreach(bool bit in Bits)
            {
                result += bit ? "1" : "0";
            }
            return result;
        }

        public bool isInDistance(int dist, Element other)
        {
            int curDist = 0;
            for(int i = 0; i < Bits.Length; ++i)
            {
                if (Bits[i] != other.Bits[i])
                {
                    curDist++;
                    if (curDist > dist)
                    {
                        return false;
                    }
                }    
            }
            return true;
        }
    }

    class Program
    {
        static Element[] elements;
        static int[] parents;
        static List<Element>[] elementsMap;

        static int Find(int index)
        {
            int result = index;
            List<int> pathCompression = new List<int>();
            while(result != parents[result])
            {
                pathCompression.Add(result);
                result = parents[result];
            }
            foreach(var i in pathCompression)
            {
                parents[i] = result;
            }
            return result;
        }

        static bool Union(int a, int b)
        {
            int aParent = Find(a);
            int bParent = Find(b);
            if (aParent != bParent)
            {
                parents[bParent] = aParent;
                return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines("clustering_big.txt");
            string[] firstLineArgs = lines[0].Split(' ');
            int count = int.Parse(firstLineArgs[0]);
            int numberOfBits = int.Parse(firstLineArgs[1]);

            elements = new Element[count];
            parents = new int[count];
            elementsMap = new List<Element>[numberOfBits];

            for(int i = 0; i < numberOfBits; ++i)
            {
                elementsMap[i] = new List<Element>();
            }

            for(int i = 0; i < count; ++i)
            {
                var element = new Element(numberOfBits);
                string[] bits = lines[i + 1].Split(' ');
                for(int j = 0; j < numberOfBits; ++j)
                {
                    bool bitValue = int.Parse(bits[j]) > 0;
                    element.SetBit(j, bitValue);
                    if (bitValue == true)
                    {
                        ++element.OnesCount;
                    }
                }
                element.Index = i;
                elements[i] = element;
                parents[i] = i;
                elementsMap[element.OnesCount - 1].Add(element);
            }

            int clustersCount = count;

            for(int j = 0; j < elementsMap.Length - 2; ++j)
            {
                List<Element> elems = elementsMap[j];
                for(int k = 0; k < elementsMap[j].Count; ++k)
                {
                    Element elem = elementsMap[j][k];
                    for(int q = k + 1; q < elems.Count; ++q)
                    {
                        if (elem.isInDistance(2, elementsMap[j][q]))
                        {
                            if (Union(elem.Index, elems[q].Index))
                            {
                                --clustersCount;
                            }
                        }
                    }


                    for(int q = 0; q < elementsMap[j+1].Count; ++q)
                    {
                        if (elem.isInDistance(2, elementsMap[j + 1][q]))
                        {
                            if (Union(elem.Index, elementsMap[j + 1][q].Index))
                            {
                                --clustersCount;
                            }
                        }

                    }

                    for (int q = 0; q < elementsMap[j + 2].Count; ++q)
                    {
                        if (elem.isInDistance(2, elementsMap[j+2][q]))
                        {
                            if (Union(elem.Index, elementsMap[j + 2][q].Index))
                            {
                                --clustersCount;
                            }
                        }
                    }
                }
            }

            Console.Write(string.Format("Result number of clusters = {0}", clustersCount)); // Correct result: 6118
        }
    }
}
