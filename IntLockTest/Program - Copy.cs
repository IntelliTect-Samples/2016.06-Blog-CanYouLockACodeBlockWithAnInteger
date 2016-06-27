using System;
using System.Threading;

namespace IntLockTest
{

    // Here's the output
    /*
        Enter IntLockTest Thread IntLock-1
        Delay IntLockTest Thread IntLock-1 sleeping (concurrent = 1)
        Enter IntLockTest Thread IntLock-2
        Delay IntLockTest Thread IntLock-2 sleeping (concurrent = 2)
        Leave IntLockTest Thread IntLock-2
        Leave IntLockTest Thread IntLock-1

        Enter IntLockTest Thread BoxedIntLock-1
        Delay IntLockTest Thread BoxedIntLock-1 sleeping (concurrent = 1)
        Enter IntLockTest Thread BoxedIntLock-2
        Leave IntLockTest Thread BoxedIntLock-1
        Delay IntLockTest Thread BoxedIntLock-2 sleeping (concurrent = 1)
        Leave IntLockTest Thread BoxedIntLock-2
    */

    class Program
    {
        static void Main(string[] args)
        {
            //var result = ReferenceEquals( (object) 1, (object) 1 );
            //Console.Out.WriteLine($"obj,obj = {result}");

            var t1 = new Thread( IntLockTest ) { Name = "IntLock-1" };
            var t2 = new Thread( IntLockTest ) { Name = "IntLock-2" };
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            Console.Out.WriteLine();

            var t3 = new Thread( BoxedIntLockTest ) { Name = "BoxedIntLock-1" };
            var t4 = new Thread( BoxedIntLockTest ) { Name = "BoxedIntLock-2" };
            t3.Start();
            t4.Start();
            t3.Join();
            t4.Join();
        }

        public static readonly int IntLock = 0;
        public static readonly object BoxedIntLock = (object) IntLock;
        public static int ConcurrentSleeper = 0;

        private static void IntLockTest( )
        {
            Console.Out.WriteLine( $"Enter IntLockTest Thread {Thread.CurrentThread.Name}" );
            lock ( (object) IntLock )
            {
                ConcurrentSleeper += 1;
                Console.Out.WriteLine( $"Delay IntLockTest Thread {Thread.CurrentThread.Name} sleeping (concurrent = {ConcurrentSleeper})" );
                Thread.Sleep( 500 );
                ConcurrentSleeper -= 1;
            }
            Console.Out.WriteLine( $"Leave IntLockTest Thread {Thread.CurrentThread.Name}" );
        }

        private static void BoxedIntLockTest( )
        {
            Console.Out.WriteLine( $"Enter IntLockTest Thread {Thread.CurrentThread.Name}" );
            lock ( BoxedIntLock )
            {
                ConcurrentSleeper += 1;
                Console.Out.WriteLine( $"Delay IntLockTest Thread {Thread.CurrentThread.Name} sleeping (concurrent = {ConcurrentSleeper})" );
                Thread.Sleep( 500 );
                ConcurrentSleeper -= 1;
            }
            Console.Out.WriteLine( $"Leave IntLockTest Thread {Thread.CurrentThread.Name}" );
        }
    }
}
