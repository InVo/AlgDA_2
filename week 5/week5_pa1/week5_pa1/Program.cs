using System;
using System.Collections.Generic;
using System.Globalization;

namespace week5_pa1
{
    class City
    {
        public int index;
        public float x;
        public float y;

        public City(int i, float x, float y)
        {
            index = i;
            this.x = x;
            this.y = y;
        }

        public double Distance(City c)
        {
            return Math.Sqrt((x - c.x) * (x - c.x) + (y - c.y) * (y - c.y));
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines("tsp.txt");
            int num = int.Parse(lines[0]);
            List<City> cities = new List<City>(num);
            for(int i = 1; i <= num; ++i)
            {
                string[] ar = lines[i].Split(' ');
                float x = float.Parse(ar[0], CultureInfo.InvariantCulture);
                float y = float.Parse(ar[1], CultureInfo.InvariantCulture);
                var city = new City(i - 1, x, y);
                cities.Add(city);
            }

            double[,] A = new double[num, num];
            for(int i = 0; i < num; ++i)
            {
                A[i, 0] = i == 0 ? 0 : float.MaxValue;
            }

            for(int i = 1; i < num; ++i) 
                for(int j = 1; j < num; ++j)
                {
                    double min = float.MaxValue;
                    for(int k = 0; k < j; ++k)
                    {
                        double val = A[i, k] + cities[k].Distance(cities[j]);
                        if (min > val)
                        {
                            min = val;
                        }
                    }
                    A[i, j] = min;
                }
            // end of i-if
            double lastMin = float.MaxValue;
            for (int j = 1; j < num; ++j)
            {
                double val = A[num - 1, j] + cities[j].Distance(cities[0]);
                if (lastMin > val)
                {
                    lastMin = val;
                }
            }
            Console.WriteLine(lastMin);
        }
    }
}
