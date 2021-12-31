using Algiers;
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
    }
}