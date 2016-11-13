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
    }

    class Program
    {
        static List<Element> elements;

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines("clustering_big.txt");
            string[] firstLineArgs = lines[0].Split(' ');
            int count = int.Parse(firstLineArgs[0]);
            int numberOfBits = int.Parse(firstLineArgs[1]);

            elements = new List<Element>(count);

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
                        element.SortingValue += (long)Math.Pow(2, j);
                    }
                }
                elements.Add(element);
            }

            elements.Sort((a, b) =>
            {
 /*               if (a.OnesCount > b.OnesCount)
                {
                    return 1;
                }
                else if (a.OnesCount == b.OnesCount)
                { */
                    if (a.SortingValue > b.SortingValue)
                    {
                        return 1;
                    }
                    else if (a.SortingValue == b.SortingValue)
                    {
                        return 0;
                    }
//                }
                return -1;
            });

            string[] stringArray = new string[count];
            for(int i = 0; i < elements.Count; ++i)
            {
                stringArray[i] = elements[i].ToString();
            }

            int c = 0;
        }
    }
}
