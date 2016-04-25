using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace TryThread
{
    class Program
    {
        static void Main()
        {
            //TryConstructors();
            //TrySleep();
            //TryJoin();
            //TryPriorities();
            //Thread tr = new Thread(ThrowException); tr.Start();
            //TryInterrupting();
            TryPool();
        }

        static void TryConstructors()
        {
            // Creates a new instances of a thread an passes on a method
            Thread t1 = new Thread(PrintNoMessage);
            t1.Name = "My first thread";
            t1.Start();

            // using a lambda expression as the parameter to the Thread constructor
            // you can give any number of parameters to the called method 
            // and the paramters can be typed (not object)
            // For example if we want to pass parameters (So not a void method)
            // http://stackoverflow.com/questions/3360555/how-to-pass-parameters-to-threadstart-method-in-thread

            // We are setting the anonomous method to refer to PrintWithgMessage1 that takes a parameter
            Thread t3 = new Thread(() => PrintWithMessage1("hello again"));
            t3.Start();

            // When we run it we get an almost random order og temp because we dont know when the OS will assign cores to the different threads
            for (int i = 0; i < 10; i++)
            {
                int temp = i;
                Thread tt = new Thread(() => Console.WriteLine(temp));
                tt.Start();
            }
        }

        static void PrintNoMessage()
        {
            Console.WriteLine("some message: " + Thread.CurrentThread.Name);
        }

        static void PrintWithMessage1(String message)
        {
            Console.WriteLine(message);
        }

        // The first thread will run onw and then sleep for 10 miliseconds. While it sleeps the second thread will run as many loops as it can before the first
        // thread "wakes" up
        static void TrySleep()
        {
            Thread thread = new Thread(() => PrintWithMessage3("Hello", 10, 10));
            Thread thread2 = new Thread(() => PrintWithMessage3("Another", 10, 1));
            thread.Start();
            thread2.Start();
        }

        static void PrintWithMessage3(String message, int howmany, int sleepTime)
        {
            for (int i = 0; i < howmany; i++)
            {
                Console.WriteLine("{0}:{1} {2}", i, message, Thread.CurrentThread.Name);
                Thread.Sleep(sleepTime);
            }
        }

        static void TryJoin()
        {
            Thread thread1 = new Thread(() => PrintWithMessage2("1 ", 50));
            Thread thread2 = new Thread(() => PrintWithMessage2("2 ", 50));
            thread2.Start();
            thread1.Start();
            // Basically makes the next thing wait.. i think..
            //thread1.Join();
            //thread2.Join();

            Console.WriteLine("Finished");
            Console.ReadLine();
        }

        static void PrintWithMessage2(String message, int howManyTimes)
        {
            for (int i = 0; i < howManyTimes; i++)
            {
                Console.Write(message);
            }
        }

        static void TryPriorities()
        {
            Thread thread1 = new Thread(() => PrintWithMessage2("1 ", 50));
            Thread thread2 = new Thread(() => PrintWithMessage2("2 ", 50));
            Thread thread3 = new Thread(() => PrintWithMessage2("3 ", 50));
            thread1.Priority = ThreadPriority.Lowest;
            thread2.Priority = ThreadPriority.Normal;
            thread3.Priority = ThreadPriority.Highest;
            thread3.Start();
            thread2.Start();
            thread1.Start();
        }

        static void ThrowException()
        {
            throw new Exception("Thread throwing an exception");
        }

        static void TryInterrupting()
        {
            Thread thread1 = new Thread(() => ToBeInterrupted(1000));
            thread1.Start();
            Thread.Sleep(1000);
            thread1.Interrupt();
        }

        // Exception must be handled inside the thread
        static void ToBeInterrupted(int sleepTime)
        {

            for (int i = 0; i < 100; i++)
            {
                try
                {
                    Console.WriteLine(i);
                    Thread.Sleep(100);
                }
                catch (ThreadInterruptedException)
                {
                    Console.WriteLine("The thread was interrupted");
                }
            }
        }

        public static void TryPool()
        {
            Console.WriteLine("TryPool");
            Task.Run(() => PrintNoMessage());
            Task.Run(() => PrintWithMessage2("Hello", 20));
            Task.Run(() => PrintWithMessage2("More hello", 20));
            //Console.ReadLine();
        }
    }
}
