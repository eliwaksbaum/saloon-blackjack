using System;
using Algiers;

namespace Saloon_Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            World world = Saloon.SetWorld();
            Console.WriteLine("");
            Console.WriteLine(world.start);
            while (!world.done)
            {
                Console.WriteLine("");
                string response = Parser.Parse(Console.ReadLine(), world);
                Console.WriteLine("");
                Console.WriteLine(response);
            }
            Console.ReadLine();
        }
    }
}
