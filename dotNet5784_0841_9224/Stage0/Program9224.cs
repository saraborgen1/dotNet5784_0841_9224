using System;
namespace Stage0
{
    partial class Program
    {
        private static void Main(string[] args)
        {
            Welcome9224();
            Welcome0841();
            Console.ReadKey();
        }

        private static void Welcome9224()
        {
            Console.WriteLine("Enter your name :");
            string name = Console.ReadLine();
            Console.WriteLine("{0},welcome to my first console application", name);
        }
        static partial void Welcome0841();
    }
}