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
        static Dictionary<uint, long>[] _results;
        static Dictionary<uint, long>[] _requests;

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines("knapsack_big.txt");
            string[] firstLineArgs = lines[0].Split(' ');

            uint totalCapacity = uint.Parse(firstLineArgs[0]);
            int itemsCount = int.Parse(firstLineArgs[1]);

            _items = new List<Item>();
            _results = new Dictionary<uint, long>[itemsCount];
            _requests = new Dictionary<uint, long>[itemsCount];

            for (int i = 1; i <= itemsCount; ++i)
            {
                string[] arguments = lines[i].Split(' ');
                uint weight = uint.Parse(arguments[0]);
                uint value = uint.Parse(arguments[1]);
                var item = new Item(weight, value);
                _items.Add(item);
                _results[i - 1] = new Dictionary<uint, long>();
                _requests[i - 1] = new Dictionary<uint, long>();
            }

            _items.Sort((a, b) =>
            {
                return a.Weight.CompareTo(b.Weight);
            });

            long result = GetSolution(itemsCount - 1, totalCapacity);
            int c = 0;
        }

        static long GetSolution(int topItem, uint  capacity)
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
        }
    }
}
