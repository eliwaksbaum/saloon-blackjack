using Algiers;
using Algiers.StartKit;
using System;

public class SPlayer : Player
{
    Random rand = new Random();
    string doorCode;
    public string DoorCode => doorCode;

    public SPlayer()
    {
        doorCode = SetCode();
    }

    string SetCode()
    {
        string[] code = new string[3];
        string[,] codes = new string[,] {{"7","7","7"}, {"6","8","7"}, {"5","9","7"}, {"5","8","8"}, {"4","8","9"}, {"3","9","9"}};
        int setI = rand.Next(6);
        int firstI = rand.Next(3);

        code[0] = codes[setI, firstI];
        code[1] = codes[setI, (firstI + 1)%3];
        code[2] = codes[setI, (firstI + 2)%3];
        return code[0]+code[1]+code[2];
    }

    new public void AddWaypoint(string newpoint)
    {
        base.AddWaypoint(newpoint);

        if ((newpoint == "firstdrink" && HasWaypoint("firstgame")) || (newpoint == "firstgame" && HasWaypoint("firstdrink")))
        {
            string stage2 = "You see one of the blackjack players get up from the table. He walks to the 'Staff Only' door," + 
                " enters a code, and walks through. While you were playing, you noticed he was wearing a red signet ring on his left hand." + 
                " You close your fist around an identical signet in your pocket. Time to get to work.";
            Parser.GetParser.AddAfterword(stage2);
            AddWaypoint("stage2");

            GameObject signet = new GameObject("signet");
            signet.SetTransitiveResponse("what", () => {
                return "A red signet ring you took from Pak after tracking him down on Titus. Showing it to the right people should help you out.";
            });
            AddToInventory(signet);
        }

        if (newpoint == "stage3")
        {
            string stage3 = "As the strange music wafts through the room, you hear the door to the saloon swing open. You turn to look, and" +
                " see who but motherfuckin Rys Lee strutting into the place. Shit.";
            Parser.GetParser.AddAfterword(stage3);
            current_room.AddObject(new Rys(this));
        }

        if (newpoint == "stage4")
        {
            string stage4 = "Rys takes a sip from the poisoned tequila and his face immediately goes red. His eyes bulge as he claws at his throat" +
                " struggling to breathe. You shout for a doctor. The thug from outside walks in, sees Rys on the floor, and carries him out of the bar." +
                " You see Rys staring at you just until the door swings closed.";
                
            Parser.GetParser.AddAfterword(stage4);
        }

        if (newpoint == "endA" || newpoint == "endB")
        {
            string endA = "You skid on the trash in the back alley and cut quick around to the front of the saloon.";
            string endB = "You burst through the front doors of the saloon.";

            string end = " The thug, still on guard, has a look like he knows something's not right about you but you hop into the cab before"
            + " he can do anything about it.\n"
            + "'So you made it, huh. Guess I'm getting my fare afterall.'\n"
            + "You tell the cabby he'll have a lot more to worry about than his fare if he doesn't step on it right fucking now, and he does.\n\n"
            + "The red neon sign fades out of view. What a dump. What a bust. You'll find him, though. You hear there's another one of these"
            + " outposts on Callisto. And a few on Mars. You'll find him.\n"
            + "The cab smells like shit.\nYou miss your scooter.\n\n"
            + "'Til next time, spaceman.";
            end = newpoint == "endA" ? endA + end : endB + end;

            World.GetWorld.done = true;
            Parser.GetParser.AddAfterword(end);
        }

        if (newpoint == "failDeath")
        {
            string whoops = "0 shots left. Fuck.\n\n"
            + "Seems like you weren't the only one counting. You turn away, but you hear their feet walking towards you."
            + " Slow. The sound of a gun being cocked fills your ears. Sorry, Pitr."
            + "\n\nTry again?";
            Parser.GetParser.AddAfterword(whoops);
            Parser.GetParser.GoRaw(Redo);
        }

        if (newpoint == "gasDeath")
        {
            string choke = "The gas gets so thick you can't really see. Or breathe. Your skin feels hot and cold at the same"
            + " time. You try to run back into the saloon behind you but you can't get your legs to move right. Hard to think now,"
            + " too. Sorry Pitr."
            + "\n\nTry again?";
            Parser.GetParser.AddAfterword(choke);
            Parser.GetParser.GoRaw(Redo);
        }
        
        string Redo(string input)
        {
            input = input.ToLower();
            if (input == "y" || input == "yes")
            {
                current_room.Delete();
                current_room = new Backroom(this);
                SetCounter("ammo", 6);
                Parser.GetParser.GoStandard();
                return Backroom.on_enter;
            }
            else if (input == "n" || input == "no")
            {
                World.GetWorld.done = true;
                return "So long, spaceman.";
            }
            else
            {
                return "Try again? (y/n)";
            }
        }
    }
}