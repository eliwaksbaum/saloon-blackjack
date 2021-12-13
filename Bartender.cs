using Algiers;

public class Bartender : GameObject
{
    public Bartender(Player player) : base("the bartender")
    {
        SetCondition("2talk1", false);
        SetCondition("proven", false);

        string Talk()
        {
            if (player.HasWaypoint("stage4"))
            {
                return player.HasWaypoint("hascode") ? "'Can I interest you in anything else, sir?'" :
                    "You're looking lucky sir, you might want to play a hand. Anything to quench your thirst?";
            }
            else if (player.HasWaypoint("stage3"))
            {
               return "You don't want to start a conversation with anyone will Rys is here."; 
            }
            else if (player.HasWaypoint("stage2"))
            {
                if (GetCondition("proven"))
                {
                    return "'What's your poison, sir?'";
                }
                else
                {
                    if (GetCondition("2talk1"))
                    {
                        return "'Look, do you want a drink or not?'";
                    }
                    else
                    {
                        SetCondition("2talk1", true);
                        return "'Still thirsty?'";
                    }
                }
            }
            else //stage1
            {
                return "'I'm not your shrink, bud. Do you want a drink or not?'";
            }
        }
        SetTransitiveResponse("talk", Talk);

        string Show(string item)
        {
            if (item == "signet")
            {
                string phrase = "'I'm most pleased to welcome a brother in arms.'";
                if (!GetCondition("proven"))
                {
                    SetCondition("proven", true);
                    phrase += "\nThe bartender slides you a playing card.";

                    GameObject card = new GameObject("two of hearts");
                    card.SetTransitiveResponse("what", () => {
                        return "The bartender gave you this card when you showed him the signet. What does it mean?";
                    });
                    player.AddToInventory(card);
                }
                return phrase;
            }
            else
            {
                return "The bartender doesn't seem interested in the " + item + ".";
            }
        }
        SetDitransitiveResponse("show", Show);

        SetTransitiveResponse("what", () => {return "The bartender is a big, bald, mean looking guy. He stands hunched over the bar, looking bored.";});
    }
}