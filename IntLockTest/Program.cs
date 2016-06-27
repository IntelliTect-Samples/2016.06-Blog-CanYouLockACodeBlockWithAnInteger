using System;
using System.Threading;

namespace IntLockTest
{

    /// <summary>
    /// This sample is in support of blog post "Can You Lock a Code Block With an Integer?" at Intellitect.com
    /// It demonstrates the ineffectiveness of casting a value type to an object vs. a static object in order
    /// to attempt locking a multi-threaded block of code.
    ///
    /// Written by Tom Faust, 6/2016
    ///
    /// </summary>
    class Program
    {
        static void Main( string[] args )
        {
            Console.Out.WriteLine();
            Console.Out.WriteLine( "Unboxed" );
            Console.Out.WriteLine();

            var t1 = new Thread( IntLockTest ) { Name = "BoxedIntLock-1" };
            var t2 = new Thread( IntLockTest ) { Name = "BoxedIntLock-2" };
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            Console.Out.WriteLine();
            Console.Out.WriteLine( "Boxed" );
            Console.Out.WriteLine();

            t1 = new Thread( BoxedIntLockTest ) { Name = "BoxedIntLock-1" };
            t2 = new Thread( BoxedIntLockTest ) { Name = "BoxedIntLock-2" };
            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();
        }

        private static int IntLock = 0;
        private static readonly object BoxedIntLock = (object)IntLock;
        private static int ConcurrentSleeper = 0;

        private static void IntLockTest()
        {
            Console.Out.WriteLine( $"Enter IntLockTest Thread {Thread.CurrentThread.Name}" );
            lock ( (object) IntLock )
            {
                ConcurrentSleeper += 1;
                Console.Out.WriteLine(
                        $"Delay IntLockTest Thread {Thread.CurrentThread.Name} sleeping (concurrent = {ConcurrentSleeper})" );
                Thread.Sleep( 500 );
                ConcurrentSleeper -= 1;
            }
            Console.Out.WriteLine( $"Leave IntLockTest Thread {Thread.CurrentThread.Name}" );
        }

        private static void BoxedIntLockTest()
        {
            Console.Out.WriteLine( $"Enter IntLockTest Thread {Thread.CurrentThread.Name}" );
            lock ( BoxedIntLock )
            {
                ConcurrentSleeper += 1;
                Console.Out.WriteLine(
                        $"Delay IntLockTest Thread {Thread.CurrentThread.Name} sleeping (concurrent = {ConcurrentSleeper})" );
                Thread.Sleep( 500 );
                ConcurrentSleeper -= 1;
            }
            Console.Out.WriteLine( $"Leave IntLockTest Thread {Thread.CurrentThread.Name}" );
        }
    }
}