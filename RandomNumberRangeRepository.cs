using System;
using System.Collections.Generic;
using System.Text;

namespace DemoPrimeNumberSearchMultiThread
{
    class RandomNumberRangeRepository : INumberRangeRepository
    {
        public void GenerateNumberRange(out long lowNumberRange, out long highNumberRange)
        {
            var rand = new Random();
            var rnd1 = rand.Next(10000000, 100000000); //between 10,000,000 and 99,999,999
            var rnd2 = rand.Next(100000000, 1000000000); //between 100,000,000 and 1,000,000,000
            lowNumberRange = rnd1;
            highNumberRange = rnd2;
            Console.WriteLine(String.Format("This thread will find prime numbers between {0} and {1}", rnd1, rnd2));
        }
    }
}
