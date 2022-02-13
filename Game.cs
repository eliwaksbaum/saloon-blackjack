using Algiers;
using Algiers.StartKit;
using System;

public class Game
{
    World world = new World();
    SPlayer player = new SPlayer();

    public World SetWorld()
    {
        world.start = "You come to in the back seat of the cab as it comes to a stop. You're here.\n(Type 'help' for a list of commands)";
        world.player = player;
        
        world.AddIntransitiveCommand("help", Help, State.Default);
        world.AddIntransitiveCommand("look", CMD.Look(player), State.Default, new string[]{"look around"});
        world.AddIntransitiveCommand("inv", Inv, State.All);
        world.AddIntransitiveCommand("quit", () => {world.done = true; return "So long, spaceman.";}, State.All);

        world.AddTransitiveCommand("examine", CMD.What(player), State.Default, "Examine what?");
        world.AddTransitiveCommand("take", CMD.Take(player), State.Default, "Take what?");
        world.AddTransitiveCommand("talk", CMD.Talk(player), State.Default, "Talk to whom?", preps: new string[]{"to"});
        world.AddTransitiveCommand("enter", CMD.Go(player, world), State.Default, "enter where?");

        
        world.AddTransitiveCommand("drink", Drink(), State.All, "Drink what?");
        
        world.AddDitransitiveCommand("use", Use, State.Default, "Use what?", new string[]{"on", "with"});
        world.AddDitransitiveCommand("give", Give, State.Default, "Give what?", new string[]{"to"});
        world.AddDitransitiveCommand("show", Show, State.Default, "Show what?", new string[]{"to"});

        Room intro = new Intro(player);
        world.AddRoom(intro);
        Room saloon = new Saloon(player);
        world.AddRoom(saloon);
        
        player.current_room = intro;
        player.AddCounter("money", 20);
        player.AddCounter("ammo", 6);

        return world;
    }

    string Help()
    {
        string instructions = 
        "inv - view your inventory\n" +
        "look - have a look around\n" +
        "examine - take a closer look at something or someone\n" +
        "enter - enter a room\n" +
        "talk (to) - talk to someone\n" +
        "take - pick something up and add it to your inventory\n" +
        "use - use a tool or machine\n" +
        "use .. on/with - use an item in your inventory with another object\n" +
        "give .. to - give an item in your inventory to someone else";
        

        if (player.HasWaypoint("stage1"))
        {
            instructions +=
            "\nbuy - buy a drink from the bar" + 
            "\ndrink - drink something in your inventory" +
            "\nplay blackjack - play a round of blackjack" +
            "\nopen - try to open the door";
        }

        if (player.HasWaypoint("stage2"))
        {
            instructions += "\nshow .. to - show an item in your inventory to someone else";
        }

        instructions += "\nhelp - see available actions\nquit - quit the game";

        if (player.HasWaypoint("stage3") && !player.HasWaypoint("stage4"))
        {
            instructions += "\nYou need to find a way to get Rys out of here.";
        }

        return instructions;
    }

    string Inv()
    {
        string inv = CMD.Inv(player)();
        int money = player.GetCounter("money");

        if (money <= 0)
        {
            return inv;
        }
        else if (inv == "Your inventory is empty.")
        {
            return money + "Ð";
        }
        else
        {
            return inv + ", " + money + "Ð";
        }
    }

    Func<string, string> Drink()
    {
        return (target) =>
        {
            if (!player.InInventory(target))
            {
                return "You don't have a " + target + " to drink";
            }
            else
            {
                GameObject drinkObj = player.GetObject(target);
                Func<string> drink = drinkObj.GetTransitiveResponse("drink");
                if (drink == null)
                {
                    return "You can't drink the " + target + ".";
                }
                else
                {
                    return drink();
                }
            }
        };
    }

    string Use(string tool, string target)
    {
        if (tool == "jukebox")
        {
            GameObject juke = player.GetFromRoom("jukebox");
            return juke.GetTransitiveResponse("use")();
        }
        else
        {
            return CMD.Use(player)(tool, target);
        }
    }

    string Give(string gift, string target)
    {
        if (gift == "signet")
        {
            return "After everything you went through to get a hold of that thing, you shouldn't just give it away.";
        }
        else
        {
            return CMD.Give(player)(gift, target);
        }
    }

    string Show(string item, string target)
    {
        if (!player.InInventory(item))
        {
            string indef = (Parser.StartsWithVowel(item))? "an " : "a ";
            return "You don't have " + indef + item + " in your inventory.";
        }
        else if (target == "")
        {
            return "Show " + item + " to whom?";
        }
        else if (!player.InRoom(target))
        {
            return "There is nobody named " + target + " here to show the " + item + " to.";
        }
        else
        {
            GameObject targetObj = player.GetFromRoom(target);
            Func<string, string> show = targetObj.GetDitransitiveResponse("show");
            string nullHandler = "You can't show the " + item + " to " + target + ".";
            if (show == null)
            {
                return nullHandler;
            }
            else
            {
                string response = show(item);
                return response == null ? nullHandler : response;
            }
        }
    }
}