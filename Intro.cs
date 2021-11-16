using Algiers;

public class Intro : Room
{
    public Intro(Player player) : base("intro")
    {
        description = "Your standing outside the saloon. The red neon sign bleeds into the dark purple sky. There's not "
            + "much around. There's a thug standing by the door and the cabby you hired is leaning against his car, smoking.";
        AddExit("saloon", "saloon");

        GameObject thug = new GameObject("thug");
            thug.SetTransitiveResponse("examine", () => {
                return "Looks like hired muscle. Here to protect the clients, or to protect against them?";
            });
            thug.SetTransitiveResponse("talk", () => {
                return "You only talk to me if you have a problem. You have a problem?";
            });
        AddObject(thug);

        GameObject cabby = new GameObject("cabby");
            cabby.SetCondition("talk1", false);
            cabby.SetTransitiveResponse("talk", () => {
                if (cabby.GetCondition("talk1"))
                {
                    cabby.SetCondition("talk1", true);
                    return "This is certainly an ... interesting part of town. You take care in there, dead people don't pay their fares.";
                }
                else
                {
                    return "Better hurry up. Meter's running.";
                }
            });
            cabby.SetTransitiveResponse("examine", () => {
                return "You had to take a cab to the spot, and pay this man who looks old enough to have been born on Earth to wait for you. You miss your scooter.";
            });
        AddObject(cabby);

        OnExit = () => {player.AddWaypoint("stage0");};
    }
}