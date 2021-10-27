using System;
using Algiers;

namespace Saloon_Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            World world = Saloon.SetWorld();
            Parser parser = new Parser(world);

            Console.WriteLine("");
            Console.WriteLine(world.start);
            while (!world.done)
            {
                Console.WriteLine("");
                string response = parser.Parse(Console.ReadLine(), world.Mode);
                Console.WriteLine("");
                Console.WriteLine(response);
            }
            Console.ReadLine();
        }
    }
}
