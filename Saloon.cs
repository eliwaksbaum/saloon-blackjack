using Algiers;
using Algiers.StartKit;
using System;

public class Saloon
{
    public static World SetWorld()
    {
        State saloonState = new State();
        State barState = new State();
        State notPlayingState = BlackJack.blackjackState.Inverse();

        World world = new World();
        world.start = "Welcome to the Saloon";

        Player player = world.player;
        player.State = saloonState;
        player.AddCounter("money");

        string PlayBlackJack()
        {
            BlackJack game = new BlackJack(world);
            return game.Start();
        }
        world.AddIntransitiveCommand("blackjack", PlayBlackJack, saloonState);
        BlackJack.AddCommands(world);
        
        string GoBar()
        {
            player.State = barState;
            return "You sit at the bar.";
        }
        world.AddIntransitiveCommand("bar", GoBar, saloonState);
        string LeaveBar()
        {
            player.State = saloonState;
            return "You get up from the bar.";
        }
        world.AddIntransitiveCommand("leave bar", LeaveBar, barState);
        
        world.AddIntransitiveCommand("look", CMD.Look(player), notPlayingState, new string[]{"look around"});
        
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
        world.AddIntransitiveCommand("inv", Inv, State.All);

        world.AddTransitiveCommand("what", CMD.What(player), notPlayingState, "What what?", preps: new string[]{"is"});
        world.AddTransitiveCommand("who", CMD.Who(player), State.All, "Who is who?", preps: new string[]{"is"});
        world.AddTransitiveCommand("take", CMD.Take(player), notPlayingState, "Take what?");
        world.AddTransitiveCommand("talk", CMD.Talk(player), State.All, "Talk to whom?");

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
        world.AddTransitiveCommand("drink", Drink(), State.All, "Drink what?");
        world.AddTransitiveCommand("buy", Bar.Buy(player), barState, "Buy what?");

        string Toss(string item)
        {
            if (player.InInventory(item))
            {
                player.RemoveFromInventory(item);
                return "You chuck the " + item;
            }
            else
            {
                return "you don't have a " + item + "in your inventory";
            }
        }
        world.AddTransitiveCommand("toss", Toss, State.All, "Toss what?");

        world.AddDitransitiveCommand("use", CMD.Use(player), notPlayingState, "Use what?", new string[]{"on", "with"});
        world.AddDitransitiveCommand("give", CMD.Give(player), notPlayingState, "Give what?", new string[]{"to"});

        player.IncrementCounter("money", 40);

        Room saloon = world.AddRoom("saloon");     

        Person bob = new Person("bob");
        string BobGive(string gift)
        {
            player.RemoveFromInventory(player.GetFromInventory(gift));
            return "Thanks!";
        }
        bob.SetDitransitiveResponse("give", BobGive);
        saloon.AddObject(bob);

        Chest box = new Chest("box");
        saloon.AddObject(box);

        GameObject bot = new GameObject("bot");
        string BotTake()
        {
            player.AddToInventory(bot);
            return "u take the bot from the box";
        }
        bot.SetTransitiveResponse("take", BotTake);
        box.AddObject(bot);

        Hoard matchbox = new Hoard("box of matches", "match");
        // string TakeBox()
        // {
        //     player.AddToInventory(matchbox);
        //     return "you pocket the box of matches. so many matches!";
        // }
        string LightMatch()
        {
            player.RemoveFromInventory("match");
            return "fwoosh";
        }
        string TakeMatch()
        {
            player.AddToInventory(matchbox.TakeMember());
            return "you take a match from the box";
        }
        matchbox.SetMemberTransitiveResponse("use", LightMatch);
        matchbox.SetMemberTransitiveResponse("take", TakeMatch);
        //matchbox.SetTransitiveCommand("take", TakeBox);
        saloon.AddObject(matchbox);

        GameObject ticket = new GameObject("ticket");
        ticket.SetTransitiveResponse("what", ()=>{return "a shiny ticket";});
        
        player.current_room = saloon;
        return world;
    }
}