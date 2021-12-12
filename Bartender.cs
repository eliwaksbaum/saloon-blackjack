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
                SetCondition("proven", true);
                return "'I'm most pleased to welcome a fellow armsman. Tonight's song is Black Diamond.'";
            }
            else
            {
                return null;
            }
        }
        SetDitransitiveResponse("show", Show);
    }
}