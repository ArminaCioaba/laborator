using System;
using System.Threading;

namespace consola_tema5
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var c=0;
            
                Console.WriteLine("Cycle no. "+ c.ToString());
                Console.WriteLine("Date: "+DateTime.Now.ToLocalTime());
                c++;
            
        }
    }
}
