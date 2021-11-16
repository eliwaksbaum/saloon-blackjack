using System;
using Algiers;
using Algiers.StartKit;

namespace Saloon_Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            World world = Game.SetWorld();
            ConsoleKit.Loop(world);
        }
    }
}
