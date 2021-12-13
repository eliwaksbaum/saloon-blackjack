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
    }
}