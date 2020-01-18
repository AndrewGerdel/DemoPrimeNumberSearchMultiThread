using System;
using System.Diagnostics;

namespace DemoPrimeNumberSearchMultiThread
{

    class Program
    {
        static object _padlock = new object();
        static int _runningThreads = 0;

        static void Main(string[] args)
        {
            //Use dependency injection to supply a number generator.  The RandomNumberRangeRepository will generate two numbers within some range.
            //Other repositories might pull from APIs, Databases, etc. 
            StartThread(new RandomNumberRangeRepository(), "Not-so-large Random Number Range");
            StartThread(new RandomLargerNumberRangeRepository(), "Large Random Number Range");

            WaitForThreadsToFinish();

            Console.WriteLine("\r\nAll threads completed.  Press any key to end...");
            Console.ReadKey();
        }

        private static void WaitForThreadsToFinish()
        {
            do
            {
                if (_runningThreads > 0)
                    System.Threading.Thread.Sleep(1000);
                else
                    break;
            } while (true);
        }

        private static void StartThread(INumberRangeRepository numberRangeRepository, string threadName)
        {
            PrimeNumberSearchModel model1 = new PrimeNumberSearchModel(threadName, numberRangeRepository);
            model1.PrimeNumberFoundEvent += Model1_PrimeNumberFoundEvent;
            model1.ThreadDoneEvent += Model1_ThreadDoneEvent;
            System.Threading.ThreadStart ts = new System.Threading.ThreadStart(model1.FindPrimeNumbers);
            System.Threading.Thread t = new System.Threading.Thread(ts);
            lock (_padlock)
            {
                _runningThreads++;
            }
            t.Start();
        }

        private static void Model1_ThreadDoneEvent(object o, PrimeNumberSearchModel.ThreadDoneEventArgs e)
        {
            Console.WriteLine(String.Format("\r\nThread [{1}] completed.  Found {0} prime numbers.", e.TotalPrimeNumbersFound, e.ThreadName));
            lock (_padlock)
            {
                _runningThreads--;
            }
        }

        private static void Model1_PrimeNumberFoundEvent(object o, PrimeNumberSearchModel.PrimeNumberFoundEventArgs e)
        {
            Console.Write(String.Format("\r{2}: Found prime number: {0}.  Total found: {1}", e.PrimeNumber, e.TotalPrimeNumbersFound, e.ThreadName));
        }
    }
}
