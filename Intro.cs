using Algiers;

public class Intro : Room
{
    public Intro(Player player) : base("intro")
    {
        description = "You stand outside the SALOON. The red neon sign bleeds into the dark purple sky. There's not "
            + "much around. A THUG is standing by the door and the CABBY you hired is leaning against his car, smoking.";
        
        AddExit("saloon", "saloon");
        AddExit("the saloon", "saloon");

        GameObject thug = new GameObject("thug");
            thug.SetTransitiveResponse("what", () => {
                return "Looks like hired muscle. Here to protect the clients, or to protect against them?";
            });
            thug.SetTransitiveResponse("talk", () => {
                return "'You only talk to me if you have a problem. You have a problem?'";
            });
        AddObject(thug);

        GameObject cabby = new GameObject("cabby");
            cabby.SetCondition("talk1", false);
            cabby.SetTransitiveResponse("talk", () => {
                if (cabby.GetCondition("talk1"))
                {
                    return "'Better hurry up. Meter's running.'";
                }
                else
                {
                    cabby.SetCondition("talk1", true);
                    return "'This is certainly an ... interesting part of town. You take care in there, dead people don't pay their fares.'";
                }
            });
            cabby.SetTransitiveResponse("what", () => {
                return "You had to take a cab to the spot, and pay this man who looks old enough to have been born on Earth to wait for you. You miss your scooter.";
            });
        AddObject(cabby);

        OnExit = () => {player.AddWaypoint("stage1"); Delete();};
    }
}