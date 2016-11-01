using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pa_week_1_2
{

    class Job
    {
        public int Weight { get; private set; }
        public int Length { get; private set; }
        public float Ratio { get { return Weight / Length; } }

        public Job(int weight, int length)
        {
            Weight = weight;
            Length = length;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines("jobs.txt");

            if (lines.Length == 0)
                return;

            int count = int.Parse(lines[0]);

            List<Job> jobs = new List<Job>();

            for(int i = 1; i <= count; ++i)
            {
                string[] arguments = lines[i].Split(' ');

                if (arguments.Length < 2) throw new FormatException(string.Format("illegal file format at line {0}", i));

                int weight = int.Parse(arguments[0]);
                int length = int.Parse(arguments[1]);
                Job job = new Job(weight, length);
                jobs.Add(job);
            }

            jobs.Sort((a, b) =>
            {
                if (b.Ratio > a.Ratio)
                {
                    return 1;
                }
                else if (b.Ratio == a.Ratio)
                {
                    return 0;
                }
                return -1;
            });
            int result = 0;
            int completionTime = 0;
            foreach(var job in jobs)
            {
                completionTime += job.Length;
                result += completionTime * job.Weight;
            }
            Console.WriteLine(string.Format("Result = {0}", result));
        }
    }
}
