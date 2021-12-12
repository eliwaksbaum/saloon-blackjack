using Algiers;

public class Bartender : GameObject
{
    public Bartender(Player player) : base("the bartender")
    {
        SetCondition("2talk1", false);
        SetCondition("proven", false);

        string Talk()
        {
            if (player.HasWaypoint("stage3"))
            {
                return "'Can I interest you in anything else, sir?'";
            }
            else if (player.HasWaypoint("stage2"))
            {
                if (GetCondition("proven"))
                {
                    return "'What's your poison, sir?'";
                }
                else
                {
                    return GetCondition("2talk1") ? "'Look, do you want a drink or not?'" : "'Still thirsty?'";
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
                    phrase += "\nThe bartender slides you a card.";

                    GameObject card = new GameObject("Two of Hearts");
                    card.SetTransitiveResponse("examine", () => {
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