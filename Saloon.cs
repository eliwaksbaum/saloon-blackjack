using Algiers;

public class Saloon : Room
{
    public Saloon(Player player) : base("saloon")
    {
        description = "A smoky, seedy looking place that doesn't seem to know what year it is. There's even a jukebox." +
            " No one's sitting at the bar, but there are a few people playing blacjack atma table in the corner.";

        //AddExit("back room", "lair");

        AddObject(new Dealer(player));
        AddObject(new Bartender(player));

        //JukeBox
    }
}