using Algiers;

public class SPlayer : Player
{
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
            string stage4 = "Rys fucking dies";
            Parser.GetParser.AddAfterword(stage4);
        }
    }
}