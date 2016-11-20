using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pa_week_3_2
{

    class Item
    {
        public uint Weight;
        public uint Value;

        public Item(uint weight, uint value)
        {
            Weight = weight;
            Value = value;
        }
    }

    class ResultEntry
    {
        public long Value;
        public long Requests;
    }

    class Program
    {
        static List<Item> _items;
        static long[] _curResults;
        static long[] _prevResults;

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines("knapsack_big.txt");
            string[] firstLineArgs = lines[0].Split(' ');

            uint totalCapacity = uint.Parse(firstLineArgs[0]);
            int itemsCount = int.Parse(firstLineArgs[1]);

            _items = new List<Item>();
            _curResults = new long[totalCapacity + 1];
            _prevResults = new long[totalCapacity + 1];

            for (int i = 1; i <= itemsCount; ++i)
            {
                string[] arguments = lines[i].Split(' ');
                uint value = uint.Parse(arguments[0]);
                uint weight = uint.Parse(arguments[1]);
                var item = new Item(weight, value);
                _items.Add(item);
            }

            _items.Sort((a, b) =>
            {
                if (a.Weight > b.Weight)
                {
                    return 1;
                } else if (a.Weight == b.Weight)
                {
                    return 0;
                }
                return -1;
            });

            for(int i = 0; i < itemsCount; ++i)
            {
                var item = _items[i];
                for(uint j = item.Weight; j <= totalCapacity; ++j)
                {
                    _curResults[j] = Math.Max(_prevResults[j], _prevResults[j - item.Weight] + item.Value);
                    long a = _curResults[j];
                }
                long[] temp = _prevResults;
                _prevResults = _curResults;
                _curResults = temp;
                Array.Clear(_curResults, 0, (int)totalCapacity);
            }

            long result = _curResults[totalCapacity]; // Result big: 4243395, small: 2493893
            int c = 0;
        }

/*        static long GetSolution(int topItem, uint  capacity)
        {
            long result = 0;
            if (topItem < 0 || capacity <= 0)
                return result;

            if (_results[topItem].ContainsKey(capacity))
            {
                result = _results[topItem][capacity];
                _results[topItem].Remove(capacity);
            } else
            {
                result = Math.Max(GetSolution(topItem - 1, capacity),
                    GetSolution(topItem - 1, capacity - _items[topItem].Weight) + _items[topItem].Value);
                _results[topItem][capacity] = result;
            }
                
            return result;
        }*/
    }
}
