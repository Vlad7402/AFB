using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 181; i++)
            {
                var rad = i * Math.PI / 180;
                Console.WriteLine(Math.Cos(rad));
            }
        }
    }
}
