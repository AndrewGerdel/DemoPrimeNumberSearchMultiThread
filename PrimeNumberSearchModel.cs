using System;
using System.Collections.Generic;
using System.Text;

namespace DemoPrimeNumberSearchMultiThread
{
    class PrimeNumberSearchModel
    {
        public event PrimeNumberFoundEventHandler PrimeNumberFoundEvent;
        public event ThreadDoneEventHandler ThreadDoneEvent;

        public delegate void PrimeNumberFoundEventHandler(object o, PrimeNumberFoundEventArgs e);
        public delegate void ThreadDoneEventHandler(object o, ThreadDoneEventArgs e);
        public class PrimeNumberFoundEventArgs : EventArgs
        {
            public long PrimeNumber;
            public int TotalPrimeNumbersFound;
            public string ThreadName;
        }
        public class ThreadDoneEventArgs : EventArgs
        {
            public int TotalPrimeNumbersFound;
            public string ThreadName;
        }

        int _totalPrimeNumbersFound = 0;
        INumberRangeRepository _numberRangeRepo;
        string _threadName = String.Empty;

        public PrimeNumberSearchModel(string threadName, INumberRangeRepository numberRangeRepo)
        {
            _numberRangeRepo = numberRangeRepo;
            _threadName = threadName;
        }

        public void FindPrimeNumbers()
        {
            long lowNumberRange;
            long highNumberRange;
            _numberRangeRepo.GenerateNumberRange(out lowNumberRange, out highNumberRange);
            for (long i = lowNumberRange; i <= highNumberRange; i++)
            {
                if (isPrime(i))
                {
                    _totalPrimeNumbersFound++;
                    if (PrimeNumberFoundEvent != null)
                        PrimeNumberFoundEvent(this, new PrimeNumberFoundEventArgs() { PrimeNumber = i, TotalPrimeNumbersFound = _totalPrimeNumbersFound, ThreadName=_threadName });
                }
            }

            if (ThreadDoneEvent != null)
                ThreadDoneEvent(this, new ThreadDoneEventArgs() { TotalPrimeNumbersFound = _totalPrimeNumbersFound, ThreadName = _threadName });
        }

        /// <summary>
        /// Stolen from https://www.geeksforgeeks.org/prime-numbers/
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private bool isPrime(long n)
        {
            // Corner case 
            if (n <= 1)
                return false;

            // Check from 2 to n-1 
            for (int i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }
    }
}
