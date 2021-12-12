using Algiers;

public class Dealer : GameObject
{
    public Dealer(Player player, State bjs) : base("the dealer")
    {
        SetCondition("proven", false);

        string Talk()
        {
            if (player.State == bjs)
            {
                return "You try to get the dealer's attention but he ignores you. A professional, it seems.";
            }
            else
            {
                if (player.HasWaypoint("stage4"))
                {
                    return "'That was quite the hand you got there.'";
                }
                else if (player.HasWaypoint("stage3"))
                {
                    player.AddWaypoint("rig game");
                    return "'You have good taste in music. Why don't you play a hand?'";
                }
                else //stage 1 or 2
                {
                    return GetCondition("proven") ? "'If you're not going to play, why don't you put on some music?'" :
                        "'I'm running a game here. Are you in or are you out?'";
                }
            }
        }
        SetTransitiveResponse("talk", Talk);
        
        string Show(string item)
        {
            if (item == "signet")
            {
                string phrase = "'I'm most pleased to welcome a brother in arms.";
                if (!GetCondition("proven"))
                {
                    SetCondition("proven", true);
                    phrase += " A little music would make it more lively in here, don't you think?'";
                }
                else
                {
                    phrase += "'";
                }
                return phrase;
            }
            else
            {
                return "The dealer doesn't seem interested in the " + item + ".";
            }
        }
        SetDitransitiveResponse("show", Show);
    }
}