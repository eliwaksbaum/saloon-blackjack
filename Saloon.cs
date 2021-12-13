using Algiers;

public class Saloon : Room
{
    public Saloon(Player player) : base("saloon")
    {
        description = "A smoky, seedy looking place that doesn't seem to know what year it is. There's even a JUKEBOX." +
            " The BARTENDER stands in front of an empty COUNTER. There's a DEALER running a game of blacjack for a few" +
            " people around a table in the corner.";

        //AddExit("back room", "lair");

        AddObject(new Dealer(player));
        AddObject(new Bartender(player));
        
        //Counter
        GameObject counter = new GameObject("counter");
        counter.SetTransitiveResponse("what", () => {
            return "The main thing you notice about the counter is that it's sticky. You also see a DRINK LIST and a BOWL OF PEANUTS.";
        });
        AddObject(counter);

        //Peanuts
        Hoard peanuts = new Hoard("bowl of peanuts", "peanut");
        peanuts.SetTransitiveResponse("what", () => {
            return "A small bowl, but it fits a surprisng number of peanuts.";
        });
        peanuts.SetMemberTransitiveResponse("what", () => {
            return "Just a peanut.";
        });
        peanuts.SetMemberTransitiveResponse("take", () => {
            player.AddToInventory(peanuts.TakeMember());
            return "You take a peanut from the bowl. You don't really care for peanuts but hey, maybe you'll run into an elephant.";
        });
        AddObject(peanuts);

        //DrinkList
        GameObject menu = new GameObject("drink list");
        menu.SetTransitiveResponse("what", () => {
            return "Whiskey - 3Ð\nTequila - 5Ð\nGin - 5Ð\nMoonshine - 8Ð\nBottle of Whiskey - 14Ð";
        });
        AddObject(menu);

        //JukeBox
    }
}