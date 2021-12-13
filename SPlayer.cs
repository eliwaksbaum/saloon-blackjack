using Algiers;

public class SPlayer : Player
{
    new public void AddWaypoint(string newpoint)
    {
        base.AddWaypoint(newpoint);

        if ((newpoint == "firstdrink" && HasWaypoint("firstgame")) || (newpoint == "firstgame" && HasWaypoint("firstdrink")))
        {
            string stage2 = "You see one of the blackjack players get up from the table. He walks to the 'Staff Only' door," + 
                " enters a code, and walks through. You saw him speaking with the bartender earlier about something.";
            Parser.GetParser.AddAfterword(stage2);
            AddWaypoint("stage2");
        }
    }
}