using Algiers;

public class Dealer : GameObject
{
    public Dealer(Player player) : base("the dealer")
    {
        SetCondition("proven", false);

        string Talk()
        {
            if (player.State == BlackJack.blackjackState)
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
                    phrase += " A little music would make it more lively in here, don't you think?'+\n A strange looking coin " +
                        " appears between the dealer's fingers, then jumps out towards you. You catch it. It says 'Quarter Dollar' on it.";
                    
                    GameObject quarter = new GameObject("quarter");
                    quarter.SetTransitiveResponse("what", () => {return "The dealer gave you this odd coin when you showed him the" +
                        " signet. What could it mean?";});
                    player.AddToInventory(quarter);
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

        SetTransitiveResponse("what", () => {return "The dealer sports a neat moustache, a wicked scar across his jaw, and a red bowtie." +
            " He makes it look the cards are moving on their own.";});
    }
}