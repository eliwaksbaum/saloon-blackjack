using System;
using Algiers;
using Algiers.StartKit;

namespace Saloon_Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            World world = Saloon.SetWorld();
            ConsoleKit.Loop(world);
        }
    }
}
